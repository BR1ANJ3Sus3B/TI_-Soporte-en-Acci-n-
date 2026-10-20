"""Genera el cronograma propuesto del primer prototipo como imagen PNG."""
from pathlib import Path

import matplotlib

matplotlib.use("Agg")
import matplotlib.pyplot as plt


def main():
    stages = [
        ("01  Diseño y alcance", "Pantallas, misión, datos y permisos", 1, 2, "#22d3ee"),
        ("02  Servicios compartidos", "API, MongoDB, acceso y progreso", 3, 2, "#2dd4bf"),
        ("03  Videojuego Unity", "Misión de Sofía y registro del resultado", 5, 2, "#a78bfa"),
        ("04  Aplicación Flutter", "Misiones, guía y progreso autenticado", 7, 2, "#38bdf8"),
        ("05  React y PWA", "Paneles y guías públicas sin conexión", 9, 2, "#fbbf24"),
        ("06  Integración y entrega", "Pruebas, correcciones y demostración", 11, 2, "#4ade80"),
    ]
    plt.rcParams.update({"font.family": "DejaVu Sans", "font.size": 11})
    fig, ax = plt.subplots(figsize=(16, 8.5), facecolor="#0b1220")
    ax.set_facecolor("#101c2e")
    for week in range(1, 13):
        if week % 2:
            ax.axvspan(week - 0.5, week + 0.5, color="#17253b", zorder=0)
    for row, (_, _, start, duration, color) in enumerate(stages):
        ax.barh(row, duration, left=start - 0.5, height=0.46, color=color, zorder=3)
        ax.text(start + (duration - 1) / 2, row, f"S{start} – S{start + duration - 1}",
                ha="center", va="center", color="#07121e", weight="bold", zorder=4)
        ax.scatter(start + duration - 0.5, row, marker="D", s=55,
                   color="white", edgecolor="#101c2e", zorder=5)
    ax.set_xlim(0.5, 12.7)
    ax.set_ylim(5.65, -0.65)
    ax.set_xticks(range(1, 13), [f"S{i}" for i in range(1, 13)])
    ax.xaxis.tick_top()
    ax.tick_params(axis="both", colors="#dbe7f5", length=0, pad=12)
    ax.set_yticks(range(6), [f"{title}\n{detail}" for title, detail, *_ in stages])
    for label in ax.get_yticklabels():
        label.set_linespacing(1.8)
    for spine in ax.spines.values():
        spine.set_visible(False)
    fig.text(0.035, 0.94, "TI: SOPORTE EN ACCIÓN", color="#22d3ee", fontsize=13, weight="bold")
    fig.text(0.035, 0.885, "Diagrama de Gantt · Primer prototipo integrado", color="white", fontsize=23, weight="bold")
    fig.text(0.035, 0.84, "Propuesta de 12 semanas · Un responsable · Secuencia de seis etapas", color="#b8c9df", fontsize=12)
    fig.text(0.035, 0.145, "◆  Hito: resultado de la etapa verificado antes de continuar", color="white", fontsize=11)
    fig.text(0.035, 0.105, "Cada etapa incluye sus pruebas y documentación; las semanas 11–12 concentran la validación integral.", color="#b8c9df", fontsize=11)
    fig.text(0.035, 0.065, "Semanas relativas al inicio acordado. Duraciones estimadas, sujetas al calendario escolar y a la disponibilidad.", color="#b8c9df", fontsize=10)
    fig.text(0.035, 0.03, "Planificación propuesta: no representa avance realizado ni fechas de entrega confirmadas. Responsable: Brian Jesús Mendoza Márquez.", color="#b8c9df", fontsize=10)
    fig.subplots_adjust(left=0.34, right=0.97, top=0.75, bottom=0.22)
    output = Path(__file__).resolve().parents[1] / "imagenes" / "gantt-primer-prototipo.png"
    fig.savefig(output, dpi=180, facecolor=fig.get_facecolor())
    plt.close(fig)
    print(output)


if __name__ == "__main__":
    main()
