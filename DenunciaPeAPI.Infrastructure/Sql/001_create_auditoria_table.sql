USE [dbDenunciaPe]
GO
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'auditoria')
BEGIN
    CREATE TABLE [dbo].[auditoria](
        [id_auditoria] [int] IDENTITY(1,1) NOT NULL,
        [fecha_evento] [datetime] NOT NULL,
        [entidad] [varchar](100) NOT NULL,
        [evento] [varchar](50) NOT NULL,
        [exitoso] [bit] NOT NULL,
        [detalle] [varchar](max) NULL,
     CONSTRAINT [PK_auditoria] PRIMARY KEY CLUSTERED
    (
        [id_auditoria] ASC
    ) ON [PRIMARY]
    ) ON [PRIMARY]
END
GO
