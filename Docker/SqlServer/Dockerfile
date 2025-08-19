# Dockerfile for SQL Server
FROM mcr.microsoft.com/mssql/server:2022-latest

# Environment variables for SQL Server configuration
ENV ACCEPT_EULA=Y
ENV MSSQL_SA_PASSWORD=Mdlrccca2!!
ENV MSSQL_PID=Express

# Optional: Create a directory for your database files if needed
WORKDIR /var/opt/mssql

# Expose the SQL Server port
EXPOSE 1433

# Health check to verify the server is running correctly
HEALTHCHECK --interval=30s --timeout=10s --start-period=60s --retries=3 \
  CMD /opt/mssql-tools/bin/sqlcmd -S localhost -U ocd-tracker -P ${MSSQL_SA_PASSWORD} -Q "SELECT 1" || exit 1