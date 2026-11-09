import type { CSSProperties } from 'react';

export function BrandSymbol({ compact = false }: { compact?: boolean }) { return <span className={`brand-symbol ${compact ? 'brand-symbol-compact' : ''}`} aria-hidden="true">TI</span>; }
export function BrandLogo({ compact = false }: { compact?: boolean }) { return <span className="brand-logo"><BrandSymbol compact={compact} /><span><strong>Soporte en Acción</strong>{!compact && <small>NovaTech Solutions</small>}</span></span>; }
export function AppIcon({ label = 'TI', style }: { label?: string; style?: CSSProperties }) { return <span className="app-icon pixel-art" style={style} aria-hidden="true">{label}</span>; }
