# Releasing Packages

This repository publishes two NuGet packages through GitHub Actions:

- `DragoAnt.Serilog.Sinks.InMemory`
- `DragoAnt.Serilog.Sinks.InMemory.Assertions`

## GitHub and NuGet setup

- `version.props` must contain the next stable base version, for example `3.1.0`.
- GitHub Actions must be enabled for the repository.
- To match the original DragoAnt convention, the preferred GitHub Actions secret name is `NUGET_ORG_API_KEY`.
- For backward compatibility, the workflows also accept `NUGET_TOKEN` if `NUGET_ORG_API_KEY` is not set.
- `GITHUB_TOKEN` does not need to be created manually; GitHub provides it automatically for workflows.

### 1. Create a NuGet.org API key

1. Sign in to [nuget.org](https://www.nuget.org/).
2. Open `Account settings` -> `API Keys`, or go directly to [nuget.org/account/apikeys](https://www.nuget.org/account/apikeys).
3. Click `Create`.
4. Give the key a clear name, for example `GitHub Actions - SerilogSinksInMemory`.
5. Select permission to `Push new packages and package versions`.
6. Scope the key to these package IDs:
   - `DragoAnt.Serilog.Sinks.InMemory`
   - `DragoAnt.Serilog.Sinks.InMemory.Assertions`
   You can also use a single pattern such as `DragoAnt.Serilog.Sinks.InMemory*` if you prefer.
7. Choose an expiration date that matches your maintenance policy.
8. Create the key and copy it immediately. NuGet only shows the full value once.

### 2. Add the API key to GitHub

1. Open the GitHub repository.
2. Go to `Settings` -> `Secrets and variables` -> `Actions`.
3. Click `New repository secret`.
4. Set `Name` to `NUGET_ORG_API_KEY`.
5. Paste the NuGet.org API key into `Secret`.
6. Save the secret.

Note:

- `NUGET_ORG_API_KEY` is the preferred secret name and matches the original DragoAnt-style GitHub workflow.
- `NUGET_TOKEN` is still supported as a fallback during migration.
- If you want to use a different secret name such as `NUGET_API_KEY`, update the workflow files first.

### 3. Confirm GitHub Actions settings

1. Open `Settings` -> `Actions` -> `General`.
2. Make sure Actions are enabled for the repository.
3. If your repository or organization restricts actions, allow the actions used by this repo, including:
   - `actions/checkout`
   - `actions/setup-dotnet`
   - `mavrosxristoforos/get-xml-info`
   - `softprops/action-gh-release`
4. No separate manual setup is required for `GITHUB_TOKEN`; the release workflows already request `contents: write` so they can create GitHub Releases.

## Prerequisites

- `version.props` must contain the next stable base version, for example `3.1.0`.
- Release and prerelease workflows run all unit tests, pack both NuGet packages, and run all integration-test projects before publishing.

## Stable release

Use the `release` workflow for a normal public release on NuGet.org and GitHub Releases.

1. Update `version.props` to the exact release version, for example `3.1.0`.
2. Merge or push the release commit to the branch you want to release from.
3. Create and push a Git tag named `releases/<version>`, for example `releases/3.1.0`.
4. GitHub Actions runs `.github/workflows/release.yml`.

What the workflow does:

- Validates that the pushed tag matches `version.props`.
- Restores, builds, and runs the solution tests in `Release`.
- Packs both NuGet packages into the workflow `packages/` folder.
- Runs all integration-test projects against the freshly packed packages.
- Creates a GitHub Release and uploads the generated `.nupkg` files as assets.
- Pushes both packages to NuGet.org.

## Beta / prerelease release

Use the `pre-release` workflow when you want NuGet prerelease packages such as `-beta.1`, `-beta.2`, or `-rc.1`.

1. Set `version.props` to the upcoming stable base version, for example `3.1.0`.
2. Open the repository in GitHub and go to `Actions` -> `pre-release`.
3. Click `Run workflow`.
4. Choose the branch or commit to release from.
5. Enter `prerelease_suffix` without a leading dash, for example:
   - `beta.1`
   - `beta.2`
   - `rc.1`
6. Start the workflow.

The workflow builds a package version in the form `<version from version.props>-<prerelease_suffix>`, for example `3.1.0-beta.1`.

What the workflow does:

- Restores, builds, and tests the repo in `Release`.
- Packs both NuGet packages with the prerelease version.
- Runs all integration-test projects against the freshly packed prerelease packages.
- Creates a GitHub prerelease tagged with the full prerelease version, for example `3.1.0-beta.1`.
- Uploads the generated `.nupkg` files to the GitHub prerelease.
- Pushes the prerelease packages to NuGet.org.

## Versioning rules

- Stable releases use the exact version from `version.props`.
- Prereleases append the workflow input suffix to the same base version from `version.props`.
- If you need more than one prerelease for the same base version, increment the suffix yourself, for example `beta.1`, then `beta.2`.
- Do not include the leading `-` in `prerelease_suffix`; the workflow adds it for you.

## Verification after publishing

- Confirm the workflow completed successfully in GitHub Actions.
- Check the GitHub Release or prerelease for both `.nupkg` assets.
- Verify the packages appear on NuGet.org with the expected version.

## Common failure cases

- Tag/version mismatch on stable release: update either the Git tag or `version.props` so they match exactly.
- Duplicate NuGet version: bump `version.props` for stable releases or use a new prerelease suffix for beta releases.
- Missing package content after future workflow edits: keep the assertions package pack step build-enabled, because it relies on copied satellite assemblies.
