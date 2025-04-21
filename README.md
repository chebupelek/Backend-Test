# Wellcome
This is the MCC Backend test task.

### Background story
We have a blog project, its previous history was scrambled by evil Grinch. Some code was lost, many bugs were introduced. Your job is to restore it and fix some bugs.

## Rules & Requirements
1. Git
     - Log your work progress (but be reasonable, you are not required to do a commit every 5 minutes, nor you can do everything in a single commit)
     - Use basic git flow, separate each task and bugfix to a respective branch (`feature/1-abc`, `bugfix/2-xyz`). Include task number in your branch names & merge commits
     - Keep branches intact. While on a real project we delete merged branches, here we are interested in how you approach solving given problems.
     - Keep `.git` folder intact when returning your work to HR.
     - Before sending your solution back, please perform a git cleanup to remove gitignored files and folders (bin & obj)
2. Codestyle
    - Adhere to existing codestyle. Treat this project like you would when working on it with a whole team.
3. Docker
    - You should know basic commands and be able to launch the project locally.
4. Everything else
    - Up to you!

# Running the project
1. Launch `docker-compose.yml` using docker compose to deploy needed PostgreSQL databases locally
2. If getting port conflicts, update `docker-compose.yml` and `appsettings.Development.json` accordingly
3. Start the dotnet project
4. Wait for seeding to finish (check logs)

# Your tasks
Here are some required improvements or problems with the project. This list is split in two parts:
1. Easy tasks
    - Things you must complete to consider this test task done
2. Medium tasks
    - Not required for considering you for a position, but if you want to show your skills, feel free to take them.

## Easy
### 1. Mailing problems [Bug]
When a new post is added to a community, we want to notify its subscribers via email about the post. 

Elf Albert has added a mail job service and everything should work, but code fails for some reason and no emails are being sent. 

You need to investigate and fix it. 

### 2. Tag authors [Feature]
Elf Belanor has implemented tags controller, but forgot to record who created each tag!

Please fix this issue and update existing code to save user's ID when creating a tag.


### 3. Posts retrieval [Bug]
Elf Deldrach has tried to get a whole thousand posts with a single request, but aged a few years waiting for it to complete! 

Please optimize `GetAllPostsAsync` to make as few DB roundtrips as possible.

### 4. More speeding-up [Bug]
Elf Indelhafxor doesn't like posts that take a lot of time reading, but filtering by read time seems to slow down post retrieval. 

Could you please somehow speed it up?

## Hard

### 5. Sessions [Feature]
Saving sessions in-memory creates problems like needing to re-login after every backend restart. Maybe we should have saved them to the DB?

Rewrite session storage to save sessions in the DB.

### 6. Shipping Containers [Feature]
Elf Drannor wanted to deploy our app to cloud, but got sick and didn't finish his task. 

Could you help him out? Create a `Dockerfile` and `docker-compose.yml` that would allow to deploy our backend with a single `docker compose up -d` call.