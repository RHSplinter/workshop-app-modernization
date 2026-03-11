# Facilitator Guide: App Modernization with GitHub Copilot

This guide is for workshop proctors and facilitators delivering the **App Modernization with GitHub Copilot** workshop.

## Workshop Snapshot

- **Total duration:** 2 hours (120 minutes)
- **Audience:** Developers, architects, and engineering leads with basic .NET experience
- **Format:** Facilitator-led introduction followed by hands-on guided lab
- **Learning outcome:** Participants build custom agents and skills, run security analysis on a legacy .NET Framework app, and modernize it to .NET 10

> [!IMPORTANT]
> The first 15-20 minutes should be facilitator-led framing and orientation. The remaining time is hands-on execution with active coaching.

## 2-Hour Run-of-Show

| Time | Duration | Segment | Facilitator Objective |
|------|----------|---------|-----------------------|
| 00:00-00:05 | 5 min | Welcome | Walk-in |
| 00:05-00:15 | 10 min | Why custom agents and skills | Explain when custom agents outperform generic prompting and how to create them |
| 00:15-00:20 | 5 min | Repo + flow walkthrough | Show workshop structure and what participants will build |
| 00:20-00:30 | 10 min | Step 01: Prerequisites | Get everyone to a working baseline |
| 00:30-00:50 | 20 min | Step 02: Security agent | Create and validate a custom security analysis agent |
| 00:50-01:10 | 20 min | Step 03: Migration skill + modernization agent | Build reusable migration knowledge and apply it |
| 01:10-01:40 | 30 min | Step 04: Execute migration | Run migration plan, validate build/run, test endpoints |
| 01:40-01:50 | 10 min | Step 05: Review and outcomes | Compare before/after and extract lessons learned |
| 01:50-02:00 | 10 min | Q&A and close | Handle questions, provide next steps |

### Timing Guardrails

- If setup is slow, reduce Step 05 reflection by 5 minutes before cutting hands-on migration time.
- Keep Step 04 protected as the largest block; this is where value is proven.
- At the 60-minute mark, all participants should be finished or nearly finished with Step 03.

## What Proctors Should Prepare Before Session

### 1. Environment readiness (day before)

- Verify the repository opens cleanly in VS Code and Codespaces.
- Confirm GitHub Copilot access for facilitator and test account.
- Validate local prerequisites on a backup machine:
  - .NET SDK 10+
  - SQL Server LocalDB or SQL Server Express
  - C# Dev Kit (if using VS Code locally)
- Run a complete dry run through Step 04 on your own fork.

### 2. Facilitation assets

- Keep one "golden" repo fork prepared at key checkpoints:
  - After Step 02 completed
  - After Step 03 completed
  - After Step 04 completed
- Prepare copy/paste prompts for:
  - Baseline security scan
  - Security agent deep-dive
  - Migration plan generation
  - Build error troubleshooting
- Keep terminal windows ready with common commands:
  - `dotnet restore`
  - `dotnet build`
  - `dotnet run --project src/PartsCatalogAPI`

### 3. Room operations

- Identify the support model: one lead facilitator plus floating helpers (recommended).
- Decide escalation path for blockers (chat, hand-raise, helper rotation).
- Plan pacing checks at minute 30, 60, and 90.

> [!TIP]
> Ask participants to pair up informally if someone gets blocked repeatedly. Pairing reduces queue pressure on proctors.

## Opening Script (15-20 minutes)

Use this sequence to keep the introduction crisp and consistent.

### 1. Why this workshop exists (3-4 min)

- Legacy modernization is hard because teams lose context across many files and decisions.
- Generic AI helps, but custom agents and skills add reusable domain expertise and consistency.

### 2. What participants will build (4-5 min)

- A custom **security agent** for vulnerability-focused analysis.
- A reusable **migration skill** for .NET Framework to .NET 10 decisions.
- A **modernization agent** that applies migration patterns with context.

### 3. Success criteria (3-4 min)

By the end, participants should be able to:

- Create and select custom agents in Copilot Chat.
- Capture migration knowledge in a reusable skill.
- Execute and validate a practical migration path.

### 4. Delivery model and expectations (3-5 min)

- This is hands-on, not lecture-heavy.
- Participants should prioritize understanding and repeatability over perfect completion.
- Proctors will provide nudges and unblockers, not do the lab for participants.

## Facilitation Guide by Workshop Step

### Step 01: Prerequisites (10 minutes)

**Goal for proctors:** ensure all participants can start confidently.

- Ask everyone to validate Copilot is active before moving on.
- Confirm at least one successful environment check command per participant.
- Flag high-risk participants early (missing SDK, Copilot sign-in issues, database unavailable).

**Common blockers:**

- Copilot not authenticated
- .NET SDK missing or wrong version
- SQL LocalDB unavailable

**Proctor action:**

- Triage quickly: if fix > 3 minutes, move participant to fallback path (Codespaces or pair with neighbor).

### Step 02: Security Agent (20 minutes)

**Goal for proctors:** participants see a clear quality gap between generic and custom analysis.

- Encourage participants to save baseline security report from generic agent for comparison.
- Validate custom agent file placement and naming conventions.
- Ask participants to request structured findings with severity and remediation.

**Checkpoint signals:**

- Participant can select custom security agent in Copilot chat dropdown.
- Participant can produce a migration-ready security report draft.

**Common blockers:**

- Agent not appearing due to path/frontmatter problems
- Generic responses because agent instructions are too vague

**Proctor nudge:**

- "Make your agent instructions specific about vulnerability classes, severity model, and expected report format."

### Step 03: Migration Skill + Modernization Agent (20 minutes)

**Goal for proctors:** participants understand skills vs agents and can use both together.

- Verify skill structure exists under `.github/skills/{SKILL-NAME}/SKILL.md`.
- Validate the modernization agent references migration process and expected output style.
- Encourage to compare outputs of security and modernization experts to understand the impact of custom agents. 

**Checkpoint signals:**

- Participant generates a migration plan with phases and validation checkpoints.
- Participant can explain one example of skill knowledge reused by the agent.

### Step 04: Execute Migration (30 minutes)

**Goal for proctors:** participants execute plan, resolve issues, and verify behavior.

- Keep focus on phase-by-phase progress, not giant one-shot rewrites.
- Prompt participants to run build and run commands after each major phase.
- Ensure SQL injection mitigation is explicitly validated.

**Required validation milestones:**

- Build succeeds (`dotnet build`).
- API runs (`dotnet run --project src/PartsCatalogAPI`).
- Swagger is reachable.
- Search endpoint tested with malicious payload and does not execute injection.

**Common blockers:**

- Package/version conflicts
- Startup pipeline configuration issues
- Routing/auth middleware order errors

**Proctor nudge:**

- "Paste exact build/runtime errors into your modernization agent and ask for root cause + fix in one response."

### Step 05: Review (10 minutes)

**Goal for proctors:** convert activity into reusable insight.

- Ask participants to compare before/after security posture.
- Discuss what custom agents improved most: consistency, speed, risk reduction.
- Capture one takeaway and one next action from each table/group.

## Fallback and Recovery Playbook

### If many participants are blocked in setup

- Pause the room for a 5-minute guided setup sweep.
- Move to Codespaces recommendation for local environment failures.
- Re-baseline start time and shorten intro/Q&A, not migration execution.

### If participants run out of time in Step 04

- Provide the prepared checkpoint branch at "post-Step 03".
- Let them complete validation and review using that branch.
- Emphasize decision quality and reproducibility over finishing every manual edit.

### If Copilot behavior is inconsistent

- Have participants rewrite prompts with explicit scope:
  - file targets
  - required output format
  - acceptance checks
- Remind participants to ask for incremental edits rather than full rewrites.

> [!NOTE]
> It is normal for outputs to vary across participants. The workshop success metric is not identical code, but successful use of agentic workflow and validated modernization outcomes.

## Proctor Checklist (Quick Reference)

Before session:

- [ ] Dry run complete through Step 04
- [ ] Golden checkpoints prepared
- [ ] Backup environment tested
- [ ] Prompt snippets ready

During session:

- [ ] Time checks at 30/60/90 minutes
- [ ] Step 02 comparison captured (generic vs custom)
- [ ] Step 03 migration plan generated
- [ ] Step 04 build/run/endpoint validation complete for most participants

After session:

- [ ] Collect top blockers and improvements
- [ ] Capture participant wins and before/after examples
- [ ] Feed recurring issues into next facilitator revision

## Suggested Prompts for Proctors to Share

### Security baseline

"Audit the PartsCatalogAPI controllers for security issues, including SQL injection, auth gaps, and insecure configuration. Return findings by severity with remediation guidance."

### Security deep dive

"Analyze SearchProducts and GetCategoryByName for SQL injection. Explain exploitability, impact, and migration-safe fixes with before/after examples."

### Migration plan

"Create a phased migration plan for PartsCatalogAPI from .NET Framework 4.8 to .NET 10, including file-by-file changes, dependencies, and validation checkpoints."

### Build troubleshooting

"Given this build/runtime error, identify root cause, propose minimal fixes, and list exact verification steps after applying changes."

## Success Metrics for Facilitators

Measure workshop success with these indicators:

- **Adoption:** Participants create both custom agents and at least one skill.
- **Execution:** Most participants complete migration execution or checkpoint-based validation.
- **Validation:** Participants can demonstrate before/after security or modernization improvements.
- **Confidence:** Participants report they can apply this workflow to a real internal project.

## Post-Workshop Recommendations

- Encourage teams to commit shared agent and skill templates into their repos.
- Run a follow-up session focused on adding tests and CI validation for modernization changes.
- Evolve this facilitator guide after each delivery using blocker data and timing observations.
