namespace Lab5.Infrastructure.DataAccess.Migrations;

public class InitialMigration : IInitialMigration
{
    public string GetUpSql =>
    """
    
    DO $$ BEGIN
    CREATE TYPE person_role AS ENUM ('admin', 'user');
    EXCEPTION
    WHEN duplicate_object THEN null;
    END $$;
    
    create table if not exists admin_password
    (
        password_value text
    );
    
    create table if not exists persons
    (
        id bigint primary key GENERATED ALWAYS AS IDENTITY ,
        person_name text UNIQUE not null,
        person_password text not null ,
        role person_role not null 
    );
    
    create table if not exists users_info
    (
        id bigint primary key,
        name text not null UNIQUE,
        password text not null,
        balance bigint
    );

    create table if not exists operations_history
    (
        operations_id bigint primary key generated always as identity ,
        user_id bigint not null,
        operation bigint,
        date DATE
    );
    
    """;
    public string DownUpSql =>
        """
        Drop  IF EXISTS table persons;

        Drop  IF EXISTS table users_info;

        Drop  IF EXISTS table operations_history;
    """;
}
