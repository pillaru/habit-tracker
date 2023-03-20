build:
	dotnet build ./app/HabitTracker
test:
	dotnet test ./tests/HabitTracker.UnitTests
migrate_up: build
	dotnet fm migrate --processor SQLite -c "Data Source=./app/HabitTracker/habit_tracker_dev.db" \
		-a "./app/HabitTracker/bin/Debug/net6.0/HabitTracker.Persistence.dll" \
		--allowDirtyAssemblies
migrate_down: build
	dotnet fm rollback --processor SQLite -c "Data Source=./app/HabitTracker/habit_tracker_dev.db" \
		-a "./app/HabitTracker/bin/Debug/net6.0/HabitTracker.Persistence.dll" \
		--allowDirtyAssemblies
migrate_ls: build
	dotnet fm list migrations --processor SQLite -c "Data Source=./app/HabitTracker/habit_tracker_dev.db" \
		-a "./app/HabitTracker/bin/Debug/net6.0/HabitTracker.Persistence.dll" \
		--allowDirtyAssemblies
fix_whitespace:
	dotnet format whitespace ./app/HabitTracker
	dotnet format whitespace ./app/HabitTracker.Domain
	dotnet format whitespace ./app/HabitTracker.Persistence
