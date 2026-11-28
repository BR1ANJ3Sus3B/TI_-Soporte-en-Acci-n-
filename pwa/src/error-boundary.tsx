import { Component, type ErrorInfo, type ReactNode } from 'react';

type State = { failed: boolean };
export class ErrorBoundary extends Component<{ children: ReactNode }, State> {
  state: State = { failed: false };
  static getDerivedStateFromError(): State { return { failed: true }; }
  componentDidCatch(error: Error, info: ErrorInfo) { console.error('Error de interfaz recuperado', error, info); }
  render() { return this.state.failed ? <main className="fatal" role="alert"><p className="eyebrow">RECUPERACIÓN DE INTERFAZ</p><h1>No pudimos cargar esta vista.</h1><p>El contenido público permanece disponible al recargar la aplicación.</p><button className="button" onClick={() => location.assign('/inicio')}>Volver al inicio</button></main> : this.props.children; }
}
