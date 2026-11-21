"""Genera los entregables de evaluación 360° adaptada del proyecto TI: Soporte en Acción."""
from pathlib import Path
from datetime import date
from openpyxl import Workbook, load_workbook
from openpyxl.styles import Font, PatternFill, Border, Side, Alignment, Protection
from openpyxl.worksheet.datavalidation import DataValidation
from openpyxl.formatting.rule import ColorScaleRule
from openpyxl.chart import BarChart, RadarChart, Reference
from openpyxl.chart.label import DataLabelList
from openpyxl.utils import get_column_letter
from docx import Document
from docx.shared import Inches, Pt, RGBColor
from docx.enum.text import WD_ALIGN_PARAGRAPH
from docx.enum.section import WD_SECTION
from docx.enum.table import WD_TABLE_ALIGNMENT, WD_CELL_VERTICAL_ALIGNMENT
from docx.oxml import OxmlElement
from docx.oxml.ns import qn
from reportlab.lib import colors
from reportlab.lib.pagesizes import letter
from reportlab.lib.styles import getSampleStyleSheet, ParagraphStyle
from reportlab.lib.enums import TA_CENTER, TA_JUSTIFY
from reportlab.platypus import SimpleDocTemplate, Paragraph, Spacer, Table, TableStyle, PageBreak

BASE = Path(__file__).resolve().parent
XLSX = BASE / "Evaluacion_360_TI_Soporte_en_Accion.xlsx"
DOCX = BASE / "Informe_Evaluacion_360_TI_Soporte_en_Accion.docx"
PDF = BASE / "Informe_Evaluacion_360_TI_Soporte_en_Accion.pdf"

NAVY, DARK, CYAN, PURPLE, GREEN, RED, WHITE, GRAY = "06132A", "0F1B33", "4AE4EC", "8B5CF6", "10B981", "EF4444", "FFFFFF", "D9E2F3"
BRIAN = "Brian Jesus Mendoza Marquez"
IDAI = "Idai Vargas Galindo"
BRIAN_COMP = ["Liderazgo", "Planeación", "Organización", "Gestión de tareas", "Toma de decisiones", "Resolución de conflictos", "Comunicación", "Conocimientos técnicos", "Calidad del software", "Cumplimiento de objetivos", "Gestión de riesgos", "Innovación", "Documentación", "Trabajo colaborativo"]
IDAI_COMP = ["Responsabilidad", "Desarrollo de software", "Calidad del trabajo", "Manejo de herramientas", "Comunicación", "Trabajo en equipo", "Participación", "Resolución de problemas", "Aprendizaje", "Documentación", "Cumplimiento de tareas", "Iniciativa", "Adaptabilidad", "Compromiso"]

def fill(c): return PatternFill("solid", fgColor=c)
thin = Side(style="thin", color="9FB3C8")
border = Border(left=thin, right=thin, top=thin, bottom=thin)

def title(ws, text, row=1):
    ws.merge_cells(start_row=row, start_column=1, end_row=row, end_column=7)
    cell = ws.cell(row, 1, text); cell.fill = fill(NAVY); cell.font = Font(color=WHITE, bold=True, size=16)
    cell.alignment = Alignment(horizontal="center", vertical="center"); ws.row_dimensions[row].height = 28

def header(ws, row, labels):
    for col, label in enumerate(labels, 1):
        c = ws.cell(row, col, label); c.fill = fill(DARK); c.font = Font(color=WHITE, bold=True)
        c.alignment = Alignment(horizontal="center", vertical="center", wrap_text=True); c.border = border
    ws.row_dimensions[row].height = 30

def setup_eval(ws, title_text, evaluator, evaluated, comps, perspective):
    ws.sheet_view.showGridLines = False
    title(ws, title_text)
    ws.merge_cells("A3:G3"); ws["A3"] = f"Evaluador: {evaluator}    |    Persona evaluada: {evaluated}"
    ws["A3"].font = Font(bold=True, color=NAVY); ws["A3"].alignment = Alignment(horizontal="center")
    ws.merge_cells("A4:G5"); ws["A4"] = ("Instrucción: califique cada criterio de 1 a 5 según la evidencia disponible. Use N/A cuando no aplique o no exista evidencia suficiente; N/A y celdas vacías no afectan los promedios. Registre observaciones, ejemplos y recomendaciones concretas.")
    ws["A4"].alignment = Alignment(wrap_text=True, vertical="center"); ws["A4"].fill = fill("E8F7F8")
    header(ws, 7, ["No.", "Competencia", "Pregunta", "Calificación\n(1–5 / N/A)", "Observaciones", "Evidencia o ejemplo", "Recomendación de mejora"])
    dv = DataValidation(type="list", formula1='"1,2,3,4,5,N/A"', allow_blank=True)
    dv.error = "Seleccione 1, 2, 3, 4, 5 o N/A."; dv.errorTitle = "Respuesta no válida"; dv.prompt = "1 Muy deficiente · 2 Deficiente · 3 Aceptable · 4 Bueno · 5 Excelente · N/A Sin evidencia"
    dv.promptTitle = "Escala de evaluación"; dv.showErrorMessage = True; dv.showInputMessage = True
    ws.add_data_validation(dv)
    for n, comp in enumerate(comps, 1):
        r = n + 7
        question = f"¿Demuestra un desempeño observable y consistente en {comp.lower()} dentro de sus responsabilidades efectivamente desempeñadas?"
        ws.cell(r, 1, n); ws.cell(r, 2, comp); ws.cell(r, 3, question)
        for col in range(1, 8):
            c = ws.cell(r, col); c.border = border; c.alignment = Alignment(vertical="top", wrap_text=True)
        ws.cell(r, 4).protection = Protection(locked=False); ws.cell(r, 5).protection = Protection(locked=False); ws.cell(r, 6).protection = Protection(locked=False); ws.cell(r, 7).protection = Protection(locked=False)
        ws.row_dimensions[r].height = 54
    dv.add(f"D8:D{7+len(comps)}")
    ws.conditional_formatting.add(f"D8:D{7+len(comps)}", ColorScaleRule(start_type="num", start_value=1, start_color=RED, mid_type="num", mid_value=3, mid_color="FFF2CC", end_type="num", end_value=5, end_color=GREEN))
    widths = [7, 23, 53, 17, 28, 30, 30]
    for i, w in enumerate(widths, 1): ws.column_dimensions[get_column_letter(i)].width = w
    ws.freeze_panes = "A8"; ws.auto_filter.ref = f"A7:G{7+len(comps)}"
    ws.protection.sheet = True; ws.protection.selectLockedCells = False; ws.protection.selectUnlockedCells = True

def metric(ws, row, label, formula, color=CYAN):
    ws.merge_cells(start_row=row, start_column=1, end_row=row, end_column=2)
    ws.cell(row, 1, label).fill = fill(DARK); ws.cell(row, 1).font = Font(color=WHITE, bold=True)
    ws.merge_cells(start_row=row, start_column=3, end_row=row, end_column=4)
    ws.cell(row, 3, formula).fill = fill(color); ws.cell(row, 3).font = Font(bold=True, size=14, color=NAVY); ws.cell(row, 3).alignment = Alignment(horizontal="center")

def result_sheet(ws, person, auto_sheet, peer_sheet, comps):
    ws.sheet_view.showGridLines = False; title(ws, f"Resultados — {person}")
    metric(ws, 3, "Promedio autoevaluación", f'=IF(COUNT(\'{auto_sheet}\'!D8:D21)=0,"",AVERAGE(\'{auto_sheet}\'!D8:D21))')
    metric(ws, 4, "Promedio evaluación recibida", f'=IF(COUNT(\'{peer_sheet}\'!D8:D21)=0,"",AVERAGE(\'{peer_sheet}\'!D8:D21))', PURPLE)
    metric(ws, 5, "Preguntas respondidas", f'=COUNT(\'{auto_sheet}\'!D8:D21)+COUNT(\'{peer_sheet}\'!D8:D21)', GREEN)
    metric(ws, 6, "% respondido", f'=IF(COUNTA(\'{auto_sheet}\'!D8:D21,\'{peer_sheet}\'!D8:D21)=0,"",COUNT(\'{auto_sheet}\'!D8:D21,\'{peer_sheet}\'!D8:D21)/28)', GREEN)
    ws["C6"].number_format = "0%"
    header(ws, 8, ["Competencia", "Autoevaluación", "Evaluación recibida", "Diferencia (Auto - Recibida)", "Indicador"])
    for i, comp in enumerate(comps, 9):
        source = i - 1
        ws.cell(i, 1, comp); ws.cell(i, 2, f'=\'{auto_sheet}\'!D{source}'); ws.cell(i, 3, f'=\'{peer_sheet}\'!D{source}')
        ws.cell(i, 4, f'=IF(OR(B{i}="",C{i}=""),"",B{i}-C{i})')
        ws.cell(i, 5, f'=IF(COUNTA(B{i}:C{i})=0,"Pendiente",IF(AVERAGE(B{i}:C{i})>=4,"Fortaleza",IF(AVERAGE(B{i}:C{i})<3,"Área de mejora","En seguimiento")))')
        for c in range(1, 6): ws.cell(i,c).border=border; ws.cell(i,c).alignment=Alignment(wrap_text=True, vertical="center")
    for col, w in {"A":27,"B":18,"C":20,"D":27,"E":20}.items(): ws.column_dimensions[col].width=w
    ws.conditional_formatting.add("D9:D22", ColorScaleRule(start_type="num", start_value=-4, start_color=RED, mid_type="num", mid_value=0, mid_color="FFFFFF", end_type="num", end_value=4, end_color=GREEN))
    ws.freeze_panes="A9"

def make_workbook():
    wb = Workbook(); wb.calculation.fullCalcOnLoad = True; wb.calculation.forceFullCalc = True
    portada = wb.active; portada.title = "Portada"
    portada.sheet_view.showGridLines = False; title(portada, "Evaluación de Desempeño 360° Adaptada")
    portada.merge_cells("A3:G3"); portada["A3"]="TI: Soporte en Acción"; portada["A3"].font=Font(size=20,bold=True,color=NAVY); portada["A3"].alignment=Alignment(horizontal="center")
    info=[("Proyecto", "Videojuego educativo sobre soporte técnico y tecnologías de la información"),("Equipo", f"{BRIAN} — Líder de equipo y autor principal\n{IDAI} — Colaboradora"),("Alcance", "Autoevaluación y evaluación mutua; formato 360° adaptado para un equipo de dos integrantes."),("Fecha de aplicación", "Por definir")]
    for r,(k,v) in enumerate(info,6): portada.cell(r,1,k).fill=fill(DARK); portada.cell(r,1).font=Font(color=WHITE,bold=True); portada.merge_cells(start_row=r,start_column=2,end_row=r,end_column=7); portada.cell(r,2,v); portada.cell(r,2).alignment=Alignment(wrap_text=True,vertical="center"); portada.row_dimensions[r].height=42
    portada.column_dimensions["A"].width=24
    for c in "BCDEFG": portada.column_dimensions[c].width=18
    ins=wb.create_sheet("Instrucciones"); ins.sheet_view.showGridLines=False; title(ins,"Instrucciones de aplicación")
    instructions=["Esta evaluación es una 360° adaptada: cada integrante se autoevalúa y recibe evaluación mutua. No es anónima ni cuenta con múltiples fuentes independientes.","Califique solo conductas, entregables y resultados que haya observado o pueda respaldar con evidencia.","Escala: 1 Muy deficiente; 2 Deficiente; 3 Aceptable; 4 Bueno; 5 Excelente; N/A No aplica o no hay evidencia suficiente.","Las celdas de calificación, observaciones, evidencia y recomendación son editables. Las fórmulas y el dashboard están protegidos.","N/A y respuestas vacías no afectan los promedios. Si no hay calificaciones numéricas, no se mostrará promedio.","Complete las cuatro hojas de cuestionario antes de consultar Resultados, Comparación y Gráficas."]
    for r,t in enumerate(instructions,4): ins.merge_cells(start_row=r,start_column=1,end_row=r,end_column=7); ins.cell(r,1,f"{r-3}. {t}"); ins.cell(r,1).alignment=Alignment(wrap_text=True,vertical="center"); ins.cell(r,1).border=border; ins.row_dimensions[r].height=37
    ins.column_dimensions["A"].width=24
    for c in "BCDEFG": ins.column_dimensions[c].width=18
    setup_eval(wb.create_sheet("Autoevaluación Brian"), "Evaluación 1 — Autoevaluación de Brian", BRIAN, BRIAN, BRIAN_COMP, "auto")
    setup_eval(wb.create_sheet("Idai evalúa a Brian"), "Evaluación 2 — Idai evalúa a Brian", IDAI, BRIAN, BRIAN_COMP, "peer")
    setup_eval(wb.create_sheet("Autoevaluación Idai"), "Evaluación 3 — Autoevaluación de Idai", IDAI, IDAI, IDAI_COMP, "auto")
    setup_eval(wb.create_sheet("Brian evalúa a Idai"), "Evaluación 4 — Brian evalúa a Idai", BRIAN, IDAI, IDAI_COMP, "peer")
    result_sheet(wb.create_sheet("Resultados Brian"), "Brian", "Autoevaluación Brian", "Idai evalúa a Brian", BRIAN_COMP)
    result_sheet(wb.create_sheet("Resultados Idai"), "Idai", "Autoevaluación Idai", "Brian evalúa a Idai", IDAI_COMP)
    comp=wb.create_sheet("Comparación general"); comp.sheet_view.showGridLines=False; title(comp,"Comparación general — sin ranking entre roles")
    header(comp,3,["Indicador","Brian","Idai","Nota"])
    rows=[("Promedio autoevaluación","='Resultados Brian'!C3","='Resultados Idai'!C3","Se interpreta según responsabilidades de cada rol."),("Promedio evaluación recibida","='Resultados Brian'!C4","='Resultados Idai'!C4","No constituye un ranking."),("Diferencia auto - recibida","=IF(OR(B4=\"\",B5=\"\"),\"\",B4-B5)","=IF(OR(C4=\"\",C5=\"\"),\"\",C4-C5)","Muestra percepción y retroalimentación."),("% respondido","='Resultados Brian'!C6","='Resultados Idai'!C6","Se calcula sin N/A como respuesta numérica.")]
    for r,row in enumerate(rows,4):
        for c,x in enumerate(row,1): comp.cell(r,c,x); comp.cell(r,c).border=border; comp.cell(r,c).alignment=Alignment(wrap_text=True)
    comp["B7"].number_format=comp["C7"].number_format="0%"
    for col,w in {"A":30,"B":18,"C":18,"D":50}.items():comp.column_dimensions[col].width=w
    gra=wb.create_sheet("Gráficas"); gra.sheet_view.showGridLines=False; title(gra,"Dashboard de evaluación")
    for pos,label,formula,color in [("A3","Promedio Brian","='Resultados Brian'!C4",CYAN),("D3","Promedio Idai","='Resultados Idai'!C4",PURPLE),("A6","% respondido Brian","='Resultados Brian'!C6",GREEN),("D6","% respondido Idai","='Resultados Idai'!C6",GREEN)]:
        cell=gra[pos]; cell.value=label; cell.fill=fill(DARK); cell.font=Font(color=WHITE,bold=True); cell.alignment=Alignment(horizontal="center"); gra.merge_cells(start_row=cell.row,start_column=cell.column,end_row=cell.row,end_column=cell.column+1)
        val=gra.cell(cell.row+1,cell.column,formula); gra.merge_cells(start_row=cell.row+1,start_column=cell.column,end_row=cell.row+1,end_column=cell.column+1); val.fill=fill(color); val.font=Font(size=18,bold=True,color=NAVY); val.alignment=Alignment(horizontal="center")
    gra["A7"].number_format=gra["D7"].number_format="0%"
    chart=BarChart(); chart.type="bar"; chart.style=10; chart.title="Promedio de evaluación recibida"; chart.y_axis.title="Integrante"; chart.x_axis.title="Calificación"; chart.height=7; chart.width=14
    chart.add_data(Reference(comp,min_col=2,max_col=3,min_row=3,max_row=5),titles_from_data=True); chart.set_categories(Reference(comp,min_col=1,min_row=4,max_row=5)); chart.legend=None; gra.add_chart(chart,"A10")
    radar=RadarChart(); radar.title="Competencias — Brian"; radar.style=26; radar.height=9; radar.width=13
    rb=wb["Resultados Brian"]; radar.add_data(Reference(rb,min_col=2,max_col=3,min_row=8,max_row=22),titles_from_data=True); radar.set_categories(Reference(rb,min_col=1,min_row=9,max_row=22)); gra.add_chart(radar,"O10")
    plan=wb.create_sheet("Plan de mejora"); plan.sheet_view.showGridLines=False; title(plan,"Plan de mejora continua — plantilla editable")
    header(plan,3,["Integrante","Competencia","Problema identificado","Acción correctiva","Responsable","Fecha objetivo","Indicador de éxito","Estado"])
    for r in range(4,19):
        for c in range(1,9): plan.cell(r,c).border=border; plan.cell(r,c).alignment=Alignment(wrap_text=True,vertical="top"); plan.cell(r,c).protection=Protection(locked=False)
        plan.row_dimensions[r].height=34
    dv_state=DataValidation(type="list",formula1='"Pendiente,En proceso,Completado"',allow_blank=True); plan.add_data_validation(dv_state); dv_state.add("H4:H18")
    for col,w in {"A":18,"B":24,"C":30,"D":32,"E":20,"F":16,"G":28,"H":17}.items():plan.column_dimensions[col].width=w
    plan.protection.sheet=True; plan.protection.selectLockedCells=False; plan.protection.selectUnlockedCells=True
    con=wb.create_sheet("Conclusiones"); con.sheet_view.showGridLines=False; title(con,"Conclusiones")
    con.merge_cells("A3:G7"); con["A3"]="Pendiente de resultados. Esta sección debe completarse tras revisar las evaluaciones respondidas, contrastar la autoevaluación con la evaluación mutua, identificar fortalezas y oportunidades sustentadas en evidencia, y acordar el plan de mejora. No se incluyen resultados ni conclusiones ficticias."; con["A3"].alignment=Alignment(wrap_text=True,vertical="top"); con["A3"].fill=fill("E8F7F8")
    for c in "ABCDEFG":con.column_dimensions[c].width=20
    wb.save(XLSX)

def set_cell_shading(cell, color):
    tcPr=cell._tc.get_or_add_tcPr(); shd=OxmlElement('w:shd'); shd.set(qn('w:fill'),color); tcPr.append(shd)
def set_repeat_table_header(row):
    trPr=row._tr.get_or_add_trPr(); el=OxmlElement('w:tblHeader'); el.set(qn('w:val'),"true"); trPr.append(el)
def footer(section):
    p=section.footer.paragraphs[0]; p.alignment=WD_ALIGN_PARAGRAPH.CENTER
    p.add_run("TI: Soporte en Acción | Evaluación 360° Adaptada | ")
    fld=OxmlElement('w:fldSimple'); fld.set(qn('w:instr'),'PAGE'); p._p.append(fld)
def add_heading(doc,text,level=1):
    p=doc.add_heading(text,level); p.paragraph_format.space_before=Pt(12); p.paragraph_format.space_after=Pt(6); return p
def para(doc,text):
    p=doc.add_paragraph(text); p.alignment=WD_ALIGN_PARAGRAPH.JUSTIFY; p.paragraph_format.space_after=Pt(7); return p

def make_docx():
    d=Document(); sec=d.sections[0]; sec.top_margin=Inches(.75); sec.bottom_margin=Inches(.7); footer(sec)
    styles=d.styles; styles['Normal'].font.name='Aptos'; styles['Normal'].font.size=Pt(10.5)
    p=d.add_paragraph(); p.alignment=WD_ALIGN_PARAGRAPH.CENTER; p.space_after=Pt(20); r=p.add_run("INFORME DE EVALUACIÓN DE DESEMPEÑO\n360° ADAPTADA"); r.bold=True; r.font.size=Pt(22); r.font.color.rgb=RGBColor(6,19,42)
    p=d.add_paragraph(); p.alignment=WD_ALIGN_PARAGRAPH.CENTER; r=p.add_run("TI: Soporte en Acción\nVideojuego educativo sobre soporte técnico y tecnologías de la información"); r.font.size=Pt(14); r.font.color.rgb=RGBColor(15,27,51)
    d.add_paragraph("\n"); p=d.add_paragraph("Documento de aplicación y registro de resultados"); p.alignment=WD_ALIGN_PARAGRAPH.CENTER
    p=d.add_paragraph(f"Fecha de emisión: {date.today().strftime('%d/%m/%Y')}"); p.alignment=WD_ALIGN_PARAGRAPH.CENTER
    d.add_page_break()
    sections=[
    ("1. Introducción","Este informe establece el instrumento formal para evaluar el desempeño del equipo de TI: Soporte en Acción. El proyecto contempla un videojuego Unity, una aplicación móvil Flutter, servicios API, autenticación, base de datos, pruebas de software y documentación."),
    ("2. Objetivo general","Medir el desempeño individual, identificar fortalezas y oportunidades de mejora, y promover una mejora continua basada en evidencia dentro del equipo."),
    ("3. Objetivos específicos","Evaluar competencias técnicas, liderazgo, colaboración, comunicación y responsabilidad; contrastar la autoevaluación con la evaluación mutua; registrar oportunidades de capacitación; y acordar acciones de mejora medibles."),
    ("4. Descripción del equipo","Brian Jesus Mendoza Marquez: Líder de equipo y autor principal.\n\nIdai Vargas Galindo: Colaboradora.\n\nLa evaluación no asigna tareas específicas sin evidencia; se valoran únicamente responsabilidades efectivamente desempeñadas."),
    ("5. Metodología","Se utiliza una evaluación 360° adaptada para un equipo de dos integrantes: autoevaluación y evaluación mutua. No se declara anonimato ni se presenta como una evaluación tradicional con múltiples fuentes independientes. Las respuestas deberán sustentarse con observaciones y evidencia cuando sea posible."),
    ("6. Instrumentos de evaluación","Se aplican cuatro cuestionarios: (1) Autoevaluación de Brian; (2) Idai evalúa a Brian; (3) Autoevaluación de Idai; y (4) Brian evalúa a Idai. Las competencias de cada persona se mantienen equivalentes entre su autoevaluación y la evaluación recibida para permitir comparación."),
    ("7. Competencias evaluadas","Brian: liderazgo, planeación, organización, gestión de tareas, toma de decisiones, resolución de conflictos, comunicación, conocimientos técnicos, calidad del software, cumplimiento de objetivos, gestión de riesgos, innovación, documentación y trabajo colaborativo.\n\nIdai: responsabilidad, desarrollo de software, calidad del trabajo, manejo de herramientas, comunicación, trabajo en equipo, participación, resolución de problemas, aprendizaje, documentación, cumplimiento de tareas, iniciativa, adaptabilidad y compromiso."),
    ("8. Escala de calificación","1 = Muy deficiente; 2 = Deficiente; 3 = Aceptable; 4 = Bueno; 5 = Excelente; N/A = No aplica o no hay evidencia suficiente. Las respuestas N/A no deben afectar los promedios."),
    ("9. Procedimiento de aplicación","Cada integrante completa primero su autoevaluación y después la evaluación mutua. En cada reactivo se registra calificación, observaciones, evidencia o ejemplo y recomendación de mejora. Posteriormente se revisan los resultados comparativos y se completa el plan de mejora solo con hallazgos reales."),
    ("10. Resultados","Pendiente de aplicación. No se incluyen calificaciones, porcentajes ni promedios ficticios. Una vez respondido el libro de Excel, los resultados por competencia y los promedios se actualizarán automáticamente."),
    ("11. Análisis comparativo","Pendiente de datos. El análisis contrastará la autoevaluación contra la evaluación recibida para cada integrante, sin establecer rankings entre líder y colaboradora, debido a que sus responsabilidades son distintas."),
    ("12. Fortalezas y oportunidades de mejora","Pendiente de resultados y evidencia documentada. Las fortalezas se identificarán con calificaciones promedio altas y las áreas de mejora con resultados bajos o diferencias que requieran diálogo y contexto."),
    ("13. Plan de mejora continua","La plantilla del libro Excel permite registrar integrante, competencia, problema identificado, acción correctiva, responsable, fecha objetivo, indicador de éxito y estado. No se proponen problemas o acciones como hechos antes de analizar las respuestas reales."),
    ("14. Conclusiones","Pendiente de la aplicación de los instrumentos. La conclusión final deberá documentar acuerdos verificables, acciones de mejora y fechas de revisión."),]
    for h,t in sections: add_heading(d,h); para(d,t)
    add_heading(d,"15. Firmas")
    table=d.add_table(rows=3, cols=2); table.alignment=WD_TABLE_ALIGNMENT.CENTER; table.style='Table Grid'
    table.cell(0,0).text=""; table.cell(0,1).text=""; table.cell(1,0).text="________________________________\nBrian Jesus Mendoza Marquez\nLíder de equipo y autor principal"; table.cell(1,1).text="________________________________\nIdai Vargas Galindo\nColaboradora"; table.cell(2,0).text="Fecha: __________________"; table.cell(2,1).text="Fecha: __________________"
    d.save(DOCX)

def make_pdf():
    styles=getSampleStyleSheet(); styles.add(ParagraphStyle(name='TituloEval',parent=styles['Title'],fontName='Helvetica-Bold',fontSize=20,leading=24,textColor=colors.HexColor('#06132A'),alignment=TA_CENTER,spaceAfter=14)); styles.add(ParagraphStyle(name='CuerpoEval',parent=styles['BodyText'],fontSize=10,leading=14,alignment=TA_JUSTIFY,spaceAfter=8)); styles.add(ParagraphStyle(name='H1Eval',parent=styles['Heading1'],fontSize=14,leading=17,textColor=colors.HexColor('#06132A'),spaceBefore=10,spaceAfter=5))
    def pg(canvas,doc): canvas.saveState(); canvas.setFont('Helvetica',8); canvas.setFillColor(colors.HexColor('#0F1B33')); canvas.drawCentredString(letter[0]/2,28,f"TI: Soporte en Acción | Evaluación 360° Adaptada | Página {doc.page}"); canvas.restoreState()
    story=[Spacer(1,110),Paragraph("INFORME DE EVALUACIÓN DE DESEMPEÑO<br/>360° ADAPTADA",styles['TituloEval']),Paragraph("TI: Soporte en Acción",ParagraphStyle('sub',parent=styles['Normal'],alignment=TA_CENTER,fontSize=15,textColor=colors.HexColor('#0F1B33'))),Spacer(1,30),Paragraph("Documento de aplicación y registro de resultados",ParagraphStyle('center',parent=styles['Normal'],alignment=TA_CENTER)),PageBreak()]
    content=[("1. Introducción","Este informe establece el instrumento formal para evaluar el desempeño del equipo de TI: Soporte en Acción, proyecto que contempla videojuego Unity, aplicación Flutter, API, autenticación, base de datos, pruebas y documentación."),("2. Objetivo general","Medir el desempeño individual, identificar fortalezas y oportunidades de mejora, y promover la mejora continua basada en evidencia."),("3. Equipo","Brian Jesus Mendoza Marquez — Líder de equipo y autor principal.<br/><br/>Idai Vargas Galindo — Colaboradora."),("4. Metodología","Evaluación 360° adaptada basada en autoevaluación y evaluación mutua para un equipo de dos integrantes. No es anónima ni corresponde a una evaluación tradicional con múltiples fuentes independientes."),("5. Instrumentos y competencias","Se aplican cuatro cuestionarios: autoevaluación de Brian, Idai evalúa a Brian, autoevaluación de Idai y Brian evalúa a Idai. Las competencias se equiparan por persona para comparar percepción propia y retroalimentación recibida."),("6. Escala de calificación","1 Muy deficiente; 2 Deficiente; 3 Aceptable; 4 Bueno; 5 Excelente; N/A No aplica o no hay evidencia suficiente. N/A no afecta promedios."),("7. Procedimiento","Registrar calificación, observaciones, evidencia y recomendaciones para cada criterio. Posteriormente se revisan los resultados y se completa el plan de mejora con hallazgos reales."),("8. Resultados, análisis y conclusiones","Pendientes de aplicación. Este informe no incluye datos, porcentajes, resultados ni conclusiones ficticias. El libro Excel actualizará los cálculos y gráficas al ingresar respuestas válidas."),("9. Plan de mejora continua","La plantilla incluye integrante, competencia, problema identificado, acción correctiva, responsable, fecha objetivo, indicador de éxito y estado. Debe completarse solo tras analizar evidencia real.")]
    for h,t in content: story += [Paragraph(h,styles['H1Eval']),Paragraph(t,styles['CuerpoEval'])]
    story += [Spacer(1,32),Paragraph("10. Firmas",styles['H1Eval']),Table([["________________________________\nBrian Jesus Mendoza Marquez\nLíder de equipo y autor principal","________________________________\nIdai Vargas Galindo\nColaboradora"],["Fecha: __________________","Fecha: __________________"]],colWidths=[250,250],style=TableStyle([('ALIGN',(0,0),(-1,-1),'CENTER'),('VALIGN',(0,0),(-1,-1),'MIDDLE'),('TOPPADDING',(0,0),(-1,-1),15)]))]
    SimpleDocTemplate(str(PDF),pagesize=letter,rightMargin=55,leftMargin=55,topMargin=55,bottomMargin=45).build(story,onFirstPage=pg,onLaterPages=pg)

if __name__ == '__main__':
    make_workbook(); make_docx(); make_pdf()
    print(f"Generados:\n- {XLSX.name}\n- {DOCX.name}\n- {PDF.name}")
