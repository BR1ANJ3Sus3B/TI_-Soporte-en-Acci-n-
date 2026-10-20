#!/usr/bin/env python3
"""Auditoría de integridad local, no prueba real de Flutter ni de Archify."""
from pathlib import Path
import json,re,struct,sys,collections,datetime
root=Path(__file__).resolve().parent.parent
report=[]
def check(ok,message):
 report.append((bool(ok),message));print(('OK' if ok else 'FALLO')+' | '+message)
files=sorted((root/'sketches').glob('P??_*.png'))
check(len(files)==20,f'20 archivos PNG individuales ({len(files)})')
ids=[p.name[:3] for p in files]
check(ids==[f'P{i:02d}' for i in range(1,21)],'IDs únicos P01 a P20, sin saltos')
for p in files:
 with p.open('rb') as f:buf=f.read(24)
 good=buf[:8]==b'\x89PNG\r\n\x1a\n' and len(buf)>=24
 if good:
  w,h=struct.unpack('>II',buf[16:24]);good=(w,h)==(1024,1536)
 check(good,f'PNG válido 1024x1536: {p.name}')
datafile=root/'prototipo'/'datos_navegacion.js'
js=datafile.read_text(encoding='utf-8')
model=json.loads(re.search(r'window\.TI_DATA\s*=\s*(\{.*\})\s*;\s*$',js,re.S).group(1))
nodes=model['nodes'];edges=model['edges'];scenarios=model['scenarios']
node_map={n['id']:n for n in nodes}
check(len(nodes)==20 and len(node_map)==20,'20 nodos distintos en datos de navegación')
check(all((root/'prototipo'/n['image']).resolve().is_file() for n in nodes),'Cada nodo enlaza a un PNG existente')
check(len(edges)>=35,f'Matriz de transiciones (cantidad: {len(edges)})')
check(all(e['from_id'] in node_map and e['to_id'] in node_map for e in edges),'Sin transiciones a nodos inexistentes')
pairs={(e['from_id'],e['to_id']) for e in edges}
check(all(all((a,b) in pairs for a,b in zip(sc['path'],sc['path'][1:])) for sc in scenarios),'Los 10 escenarios siguen flechas existentes')
allowed=lambda n,role: any(r in ('Público','Autenticado',role) for r in n['roles'])
check(all(all(allowed(node_map[id],sc['role']) for id in sc['path']) for sc in scenarios),'Los recorridos de ejemplo respetan roles')
all_reachable=set(['P01']);work=['P01'];adj=collections.defaultdict(list)
for e in edges:adj[e['from_id']].append(e['to_id'])
while work:
 for nxt in adj[work.pop()]:
  if nxt not in all_reachable:all_reachable.add(nxt);work.append(nxt)
check(len(all_reachable)==20,'Los 20 nodos son alcanzables desde Splash en el mapa completo')
for f in ['index.html','prototipo/index.html','diagramas/navegacion_general.mmd','diagramas/navegacion_estatica.svg','docs/README_15_ACTIVIDADES.md','docs/04_fichas_20_sketches.md','docs/06_plan_validacion.md','docs/08_prompt_archify.md','docs/09_guia_github_entrega.md']:
 check((root/f).is_file(),f'Archivo requerido: {f}')
check('Archify' in (root/'docs'/'08_prompt_archify.md').read_text(),'Prompt de Archify presente')
check(all((root/'docs'/f'{i:02d}_{suffix}').is_file() for i,suffix in [ (1,'analisis_requerimientos.md'),(2,'roles_y_permisos.md'),(3,'arquitectura_navegacion.md'),(4,'fichas_20_sketches.md'),(5,'matriz_transiciones.md'),(6,'plan_validacion.md'),(7,'proceso_decisiones.md'),(8,'prompt_archify.md'),(9,'guia_github_entrega.md')]),'Nueve capítulos temáticos y guía de 15 actividades')
print('\nRESULTADO:',sum(ok for ok,_ in report),'/',len(report),'verificaciones mecánicas superadas.')
sys.exit(0 if all(ok for ok,_ in report) else 1)
