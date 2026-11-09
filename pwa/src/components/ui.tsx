import type { ButtonHTMLAttributes, InputHTMLAttributes, ReactNode } from 'react';
import { Award, CheckCircle2, Code2, Database, Headphones, Info, LoaderCircle, Network, Shield, TriangleAlert, XCircle, type LucideIcon } from 'lucide-react';

export function Card({ children, className = '' }: { children: ReactNode; className?: string }) { return <section className={`card ${className}`}>{children}</section>; }
export function Button({ children, className = '', ...props }: ButtonHTMLAttributes<HTMLButtonElement>) { return <button className={`button ${className}`} {...props}>{children}</button>; }
export function Input({ label, error, required, id, ...props }: InputHTMLAttributes<HTMLInputElement> & { label: string; error?: string; required?: boolean }) {
  const inputId = id ?? props.name;
  return <label className="form-field" htmlFor={inputId}>{label}{required && <span aria-hidden="true"> *</span>}<input id={inputId} required={required} aria-invalid={Boolean(error)} aria-describedby={error ? `${inputId}-error` : undefined} {...props} />{error && <small id={`${inputId}-error`} role="alert">{error}</small>}</label>;
}
export function ProgressBar({ value }: { value: number }) { return <div className="progress-track" aria-label={`${value}% completado`} role="progressbar" aria-valuenow={value} aria-valuemin={0} aria-valuemax={100}><span style={{ width: `${value}%` }} /></div>; }
export const skillIcons: Record<string, LucideIcon> = { headset: Headphones, network: Network, shield: Shield, code: Code2, database: Database };
export const achievementIcons: Record<string, LucideIcon> = { award: Award, shield: Shield, rocket: Award };
export type StatusTone = 'success' | 'warning' | 'error' | 'info' | 'pending' | 'locked';
const statusIcons: Record<StatusTone, LucideIcon> = { success: CheckCircle2, warning: TriangleAlert, error: XCircle, info: Info, pending: LoaderCircle, locked: Shield };
export function StatusBadge({ label, tone = 'info' }: { label: string; tone?: StatusTone }) { const Icon = statusIcons[tone]; return <span className={`status-badge status-${tone}`}><Icon size={14} aria-hidden="true" />{label}</span>; }
export function AlertMessage({ title, children, tone = 'info' }: { title: string; children: ReactNode; tone?: StatusTone }) { return <div className={`alert-message alert-${tone}`} role="status"><StatusBadge label={title} tone={tone} /><span>{children}</span></div>; }
export function EmptyState({ title, description }: { title: string; description: string }) { return <Card className="empty-state"><Info /><h2>{title}</h2><p>{description}</p></Card>; }
export function LoadingState({ label = 'Cargando información…' }: { label?: string }) { return <div className="loading-state" role="status"><LoaderCircle aria-hidden="true" /> <span>{label}</span></div>; }
export function ErrorState({ title = 'No se pudo cargar la información', description }: { title?: string; description: string }) { return <AlertMessage title={title} tone="error">{description}</AlertMessage>; }
export function AchievementBadge({ label, unlocked = true }: { label: string; unlocked?: boolean }) { return <span className={`achievement-badge ${unlocked ? '' : 'is-locked'}`}><Award aria-hidden="true" />{label}</span>; }
export function ConnectionStatus({ online = true }: { online?: boolean }) { return <StatusBadge label={online ? 'Conectado localmente' : 'Sin conexión'} tone={online ? 'success' : 'warning'} />; }
