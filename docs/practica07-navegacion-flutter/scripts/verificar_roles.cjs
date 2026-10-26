/* Ejecutar con Node y Edge instalado. Genera capturas desde la interfaz HTML. */
const fs = require('node:fs');
const path = require('node:path');
const {pathToFileURL} = require('node:url');
const {spawn} = require('node:child_process');
const assert = require('node:assert/strict');
const root = path.resolve(__dirname, '..');
const profile = fs.mkdtempSync(path.join(require('node:os').tmpdir(), 'ti-roles-'));
const edge = spawn('C:/Program Files (x86)/Microsoft/Edge/Application/msedge.exe', ['--headless=new','--disable-gpu','--no-first-run','--remote-debugging-port=0',`--user-data-dir=${profile}`,'about:blank'], {windowsHide:true,stdio:'ignore'});
const delay = ms => new Promise(r=>setTimeout(r,ms));
(async()=>{
 let socket;
 try {
  const portFile=path.join(profile,'DevToolsActivePort');
  for(let i=0;!fs.existsSync(portFile)&&i<100;i++)await delay(100);
  const port=fs.readFileSync(portFile,'utf8').split('\n')[0];
  const targets=await (await fetch(`http://127.0.0.1:${port}/json`)).json();
  socket=new WebSocket(targets.find(t=>t.type==='page').webSocketDebuggerUrl);
  await new Promise((resolve,reject)=>{socket.onopen=resolve;socket.onerror=reject});
  let sequence=0;const pending=new Map();const errors=[];
  socket.onmessage=({data})=>{const m=JSON.parse(data);if(m.method==='Runtime.exceptionThrown')errors.push(m.params.exceptionDetails);if(m.id){const p=pending.get(m.id);pending.delete(m.id);m.error?p.reject(m.error):p.resolve(m.result)}};
  const call=(method,params={})=>new Promise((resolve,reject)=>{const id=++sequence;pending.set(id,{resolve,reject});socket.send(JSON.stringify({id,method,params}))});
  const evaluate=async expression=>{const r=await call('Runtime.evaluate',{expression,returnByValue:true,awaitPromise:true});if(r.exceptionDetails)throw Error(JSON.stringify(r.exceptionDetails));return r.result.value};
  await call('Runtime.enable');await call('Page.enable');
  await call('Emulation.setDeviceMetricsOverride',{width:1440,height:1100,deviceScaleFactor:2,mobile:false});
  const visit=async file=>{await call('Page.navigate',{url:pathToFileURL(path.join(root,file)).href});await delay(700)};
  await visit('mockups-color/index.html');
  const roles=await evaluate('Object.keys(TI_ROLES)');
  if(process.argv.includes('--navegacion')){
   await visit('wireframes/navegacion.html');
   for(const role of roles){
    await evaluate(`$('role').value=${JSON.stringify(role)};$('role').onchange()`);
    assert.equal(await evaluate('current'),'P09');
    await evaluate("$('restart').click();$('phone').querySelector('[data-go=\"P02\"]').click();$('phone').querySelector('[data-go=\"P03\"]').click();$('phone').querySelector('[data-go=\"P09\"]').click()");
    assert.equal(await evaluate('current'),'P09');
    await evaluate("$('phone').querySelector('[data-go=\"'+roleConfig().module+'\"]').click()");
    assert.equal(await evaluate('current'),await evaluate('roleConfig().module'));
    await evaluate("$('back').click()");assert.equal(await evaluate('current'),'P09');
    const ids=await evaluate('PAN.filter(([id])=>allowedScreen(id)).map(([id])=>id)');
    for(const id of ids){
     await evaluate(`go(${JSON.stringify(id)});$('mode').value='original';$('mode').onchange()`);await delay(60);
     assert(await evaluate('$(' + JSON.stringify('phone') + ').querySelector("img").naturalWidth>0'),role+' '+id);
     await evaluate("$('mode').value='interactive';$('mode').onchange()");
     assert(await evaluate("[...$('phone').querySelectorAll('[data-go]:not(:disabled)')].every(b=>allowedScreen(b.dataset.go))"));
    }
    await evaluate("go('P16');$('phone').querySelector('[data-go=\"P03\"]').click()");assert.equal(await evaluate('current'),'P03');assert.equal(await evaluate('trail.length'),0);
    console.log('OK navegación, historial, sketches y cierre: '+role);
   }
   await call('Emulation.setDeviceMetricsOverride',{width:390,height:844,deviceScaleFactor:1,mobile:true});
   assert(await evaluate('document.documentElement.scrollWidth<=innerWidth'));
   assert.deepEqual(errors,[]);console.log('OK vista móvil sin desbordamiento y sin errores JavaScript.');
   await call('Browser.close');return;
  }
  if(process.argv.includes('--wireframes')){
   const screens=await evaluate('PAN.map(([id,title])=>({id,title}))');
   const dir=path.join(root,'wireframes','pantallas');fs.mkdirSync(dir,{recursive:true});
   for(const {id,title} of screens){
    await evaluate(`activeRole=Object.keys(TI_ROLES).find(r=>TI_ROLES[r].module===${JSON.stringify(id)})||'Estudiante/Jugador';picker.value=activeRole;setViewMode('normal');show(${JSON.stringify(id)});phone.classList.add('role-sketch','wireframe');phone.style.height='846px';`);
    assert.equal(await evaluate('current'),id);
    await evaluate("phone.style.height=(846+Math.max(0,phone.querySelector('.appcontent').scrollHeight-phone.querySelector('.appcontent').clientHeight))+'px';phone.scrollIntoView({block:'start'})");
    const rect=await evaluate('(()=>{const r=phone.getBoundingClientRect();return {x:r.x+scrollX,y:r.y+scrollY,width:r.width,height:r.height,scale:1}})()');
    const shot=await call('Page.captureScreenshot',{format:'png',clip:rect,captureBeyondViewport:true});
    fs.writeFileSync(path.join(dir,`${id}.png`),Buffer.from(shot.data,'base64'));
    console.log(`OK wireframe ${id}: ${title}`);
   }
   await visit('wireframes/index.html');
   assert.equal(await evaluate('document.querySelectorAll("#gallery article").length'),20);
   assert(await evaluate('[...document.querySelectorAll("#gallery img")].every(i=>i.complete&&i.naturalWidth>0)'));
   assert.deepEqual(errors,[]);console.log('OK galería: 20 imágenes cargadas, sin errores JavaScript.');
   await call('Browser.close');return;
  }
  for(const role of roles){
   await evaluate(`activeRole=${JSON.stringify(role)};picker.value=activeRole;setViewMode('normal');show('P03');phone.querySelector('[data-go="P09"]').click()`);
   assert.equal(await evaluate('current'),'P09');
   assert.equal(await evaluate('phone.querySelector("h2").textContent'),await evaluate('roleConfig().title'));
   assert(await evaluate('[...phone.querySelectorAll(".bottomnav [data-go]")].every(b=>allowedScreen(b.dataset.go))'));
   const key=await evaluate('roleConfig().key');
   await evaluate("document.getElementById('phone').scrollIntoView({block:'center'})");
   for(const mode of ['normal','lapiz']){
    await evaluate(`phone.classList.toggle('role-sketch',${mode==='lapiz'})`);
    const rect=await evaluate('(()=>{const r=phone.getBoundingClientRect();return {x:r.x+scrollX,y:r.y+scrollY,width:r.width,height:r.height,scale:1}})()');
    const shot=await call('Page.captureScreenshot',{format:'png',clip:rect,captureBeyondViewport:true});
    const dir=path.join(root,mode==='normal'?'mockups-color':'sketches','roles');fs.mkdirSync(dir,{recursive:true});fs.writeFileSync(path.join(dir,`P09_${key}.png`),Buffer.from(shot.data,'base64'));
   }
   await evaluate("phone.classList.remove('role-sketch');show(roleConfig().module);phone.querySelector('.bottomnav [data-go=\"P09\"]').click()");assert.equal(await evaluate('current'),'P09');
   await evaluate("show('P16')");assert(await evaluate('phone.textContent.includes(activeRole)'));
   await evaluate("phone.querySelector('[data-go=\"P03\"]').click()");assert.equal(await evaluate('current'),'P03');
   await evaluate("show('P09');setViewMode('lapiz')");await delay(150);
   assert(await evaluate('phone.querySelector("img").naturalWidth>0'));
   await evaluate("setViewMode('normal');show(activeRole==='Administrador'?'P17':'P18')");assert.equal(await evaluate('current'),'P09');
   console.log('OK inicio, menú, perfil, cierre, restricción e imágenes: '+role);
  }
  await visit('prototipo/index.html');
  for(const role of roles){
   await evaluate(`document.getElementById('role').value=${JSON.stringify(role)};document.getElementById('role').dispatchEvent(new Event('change'))`);await delay(150);
   assert(await evaluate('document.querySelector("#detail h2").textContent.startsWith("P09")'));
   assert(await evaluate('document.querySelector("#detail img").naturalWidth>0'));
   assert.equal(await evaluate('new URL([...document.querySelectorAll("#detail a")].find(a=>a.textContent.includes("Navegar")).href).searchParams.get("role")'),role);
  }
  assert.deepEqual(errors,[]);console.log('OK diagrama: cinco imágenes y enlaces con rol. Sin errores JavaScript.');
  await call('Browser.close');
 } finally {if(socket)socket.close();edge.kill();}
})().catch(e=>{console.error(e);process.exitCode=1});
