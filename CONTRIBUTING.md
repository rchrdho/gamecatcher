# Contributing
Please consult this guide when contributing.

## Branching model
- Base Branch: `main`
- Development Branch: `dev`
- Branch types: 
    - feat: `feat/<feature-or-scope>-<short-desc>`
    - database: `db/<feature-or-scope>-<short-desc>`
    - fix: `fix/<feature-or-scope>-<short-desc>`
    - docs: `docs/<feature-or-scope>-<short-desc>`

Example: 
- `feat/mvc-skeleton`
- `docs/readme-update`

## Commit Messages
Structure: `commit-type(scope): subject`

Commit-Types: 
- feat: new feature
- db: database related changes
- test: adding unit tests or updating tests
- docs: updating or adding documentations
- revert: revert previous commit 
- refactor: code changes that doesn't fix or add a new feature (example: fixing styling)
- delete: file deletion

Examples: 
- delete(node_modules): deleted node_modules
- docs(readme): updated readme
- fix(dashboard): resolved game card not displaying
- feat(oauth): added oauth 