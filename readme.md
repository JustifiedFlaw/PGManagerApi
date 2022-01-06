# PGManager
## Required Storage Database
Migrations will take care of creating the tables, but you will need an existing Postgresql database for storage of users and database connections.
### Storage Database Settings in launch.json
````json
{
    "configurations": [
        {
            "env": {
                "ASPNETCORE_ENVIRONMENT": "Development",
                "STORAGE_HOST": "",
                "STORAGE_PORT": "5432",
                "STORAGE_DB": "",
                "STORAGE_USER": "",
                "STORAGE_PWRD": ""
            }
        }
    ]
}
````