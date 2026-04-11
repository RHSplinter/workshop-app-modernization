---
applyTo: "**/*.md,docs/**"
---

# Workshop Content Development Guidelines

This file contains guidelines for creating and maintaining workshop content, including markdown exercises and the HTML publishing framework.

## Workshop Content Guidelines

- Workshop steps are in the `workshop/` directory as numbered markdown files
- Keep exercises focused and achievable within the stated time
- Include clear success criteria for each exercise
- Use GitHub-flavored markdown alerts (`> [!NOTE]`, `> [!TIP]`, etc.)
- Place images in `workshop/images/`

## Publishing

- The `docs/` directory contains the HTML publishing framework
- Update `docs/index.html` and `docs/step.html` when adding/removing workshop steps
- The site deploys automatically to GitHub Pages on push to `main`

## Repository Structure

- `docs/`: Published HTML site (landing page + step viewer)
- `src/`: Source code for the application used during the workshop
- `workshop/`: Source markdown lessons (numbered `00-`, `01-`, etc.)
- `.github/workflows/deploy.yml`: GitHub Pages deployment workflow
- `.github/copilot-instructions.md`: General Copilot context config
- `.github/instructions/`: Language/framework-specific instruction files
