# Copilot Instructions

## Linee guida del progetto
- Fix Dependabot NuGet vulnerabilities in 'sdk-style-conversion-negozioluca' branch of StrumentiMusicali: apply to the 10 projects in SturmentiMusicaliApp.sln, upgrade only critical/safe packages that avoid breaking changes (e.g. NLog, Newtonsoft.Json), balanced risk tolerance meaning verify with build/test after each upgrade.