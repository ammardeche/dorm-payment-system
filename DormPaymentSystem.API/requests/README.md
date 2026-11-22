# HTTP requests (httpyac)

## Folders
| Folder | What is inside |
|---|---|
| `setup/` | Run once, in number order, after every database drop. Logins, staff, seed data. |
| `admin/` | The staff API (`/api/admin/*`). One folder per feature. |
| `app/` | The student mobile app API (`/api/app/*`). No `X-Building-Id` header. |
| `tests/` | Scenarios that check a rule (multi-building, guest document). Expected result is written above each request. |

Rule: the folder name follows the URL. `/api/admin/guest` -> `admin/guests/`.
Rule: file names are lower-case with dashes, one feature per folder, `###` between requests.

## Variables
| Name | Where it comes from |
|---|---|
| `baseUrl`, `buildingId` | `.httpyac.json` (copy `.httpyac.example.json`, it is NOT committed) |
| `adminToken`, `managerToken`, `receptionistToken`, `annaToken`, `bobToken` | saved by the login scripts in `setup/` |
| `managerId`, `receptionistId` | saved by `setup/02-staff.http` |

Never write a token into a file. If the saved tokens are not visible in other files, copy them into `.httpyac.json`.

## Who is who
- admin@dorm.com: **Admin** (all buildings, only role that can use `register-staff` and `admin/user/*`)
- Marek, manager.a@dorm.local: **Manager** (only buildings he was given)
- Anna s098001, Bob s098002: **students** (mobile app)

## First run on an empty database
setup/01 -> 02 -> 03 -> 04, then any file in `tests/`.
