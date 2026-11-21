"""Servidor local para capturar la evaluación HTML y actualizar el libro Excel en cada guardado."""
from http.server import ThreadingHTTPServer, SimpleHTTPRequestHandler
from pathlib import Path
from urllib.parse import urlparse
import json
from openpyxl import load_workbook

BASE = Path(__file__).resolve().parent
STATE = BASE / "respuestas_evaluacion.json"
BOOK = BASE / "Evaluacion_360_TI_Soporte_en_Accion.xlsx"
SHEETS = {
    "brian_auto": "Autoevaluación Brian", "brian_peer": "Idai evalúa a Brian",
    "idai_auto": "Autoevaluación Idai", "idai_peer": "Brian evalúa a Idai",
}

def initial_state():
    return {key: [{"rating":"", "notes":"", "evidence":"", "recommendation":""} for _ in range(14)] for key in SHEETS}

def read_state():
    if not STATE.exists(): return initial_state()
    try:
        state = json.loads(STATE.read_text(encoding="utf-8"))
        baseline = initial_state()
        for key in baseline:
            if isinstance(state.get(key), list):
                for i, item in enumerate(state[key][:14]):
                    if isinstance(item, dict): baseline[key][i].update({k: str(item.get(k, "")) for k in baseline[key][i]})
        return baseline
    except (json.JSONDecodeError, OSError): return initial_state()

def normalize(payload):
    baseline = initial_state()
    if not isinstance(payload, dict): return baseline
    for key in baseline:
        if isinstance(payload.get(key), list):
            for i, item in enumerate(payload[key][:14]):
                if isinstance(item, dict):
                    for field in baseline[key][i]: baseline[key][i][field] = str(item.get(field, ""))[:4000]
    return baseline

def update_workbook(state):
    if not BOOK.exists(): raise FileNotFoundError("No se encontró el libro Excel. Ejecute generar_evaluacion_360.py primero.")
    wb = load_workbook(BOOK)
    for key, sheet_name in SHEETS.items():
        ws = wb[sheet_name]
        for i, answer in enumerate(state[key], start=8):
            rating = answer["rating"]
            ws.cell(i, 4).value = rating if rating in {"1", "2", "3", "4", "5", "N/A"} else None
            ws.cell(i, 5).value = answer["notes"] or None
            ws.cell(i, 6).value = answer["evidence"] or None
            ws.cell(i, 7).value = answer["recommendation"] or None
    wb.calculation.fullCalcOnLoad = True; wb.calculation.forceFullCalc = True
    wb.save(BOOK)

class Handler(SimpleHTTPRequestHandler):
    def __init__(self, *args, **kwargs): super().__init__(*args, directory=str(BASE), **kwargs)
    def send_json(self, data, status=200):
        raw=json.dumps(data, ensure_ascii=False).encode("utf-8"); self.send_response(status); self.send_header("Content-Type","application/json; charset=utf-8"); self.send_header("Content-Length",str(len(raw))); self.end_headers(); self.wfile.write(raw)
    def do_GET(self):
        path=urlparse(self.path).path
        if path == "/api/state": return self.send_json(read_state())
        if path in {"/", "/index.html"}:
            self.path="/evaluacion_360.html"
        return super().do_GET()
    def do_POST(self):
        if urlparse(self.path).path != "/api/state": return self.send_error(404)
        try:
            length=int(self.headers.get("Content-Length","0")); state=normalize(json.loads(self.rfile.read(length).decode("utf-8")))
            STATE.write_text(json.dumps(state,ensure_ascii=False,indent=2),encoding="utf-8")
            update_workbook(state)
            self.send_json({"ok":True,"message":"Datos guardados y Excel actualizado."})
        except Exception as exc: self.send_json({"ok":False,"message":str(exc)},500)

if __name__ == "__main__":
    print("Evaluación disponible en http://localhost:8765")
    print("Cada guardado actualiza respuestas_evaluacion.json y el archivo Excel.")
    ThreadingHTTPServer(("127.0.0.1",8765),Handler).serve_forever()
