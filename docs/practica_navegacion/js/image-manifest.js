// Inventario de los recursos reales; rutas relativas a docs/practica_navegacion/.
const imageManifest=[...generalScreens,...roleScreens].map(({id,nombre,comparison,roles})=>({id,nombre,roles,comparison}));
const imageInventory={general:generalScreens.length,roles:roleScreens.length,total:imageManifest.length};
