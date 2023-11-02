USE DCMMeasure;

DROP TABLE Measurement;

CREATE TABLE [dbo].[Measurement](
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [InstanceUID] [varchar](50) NULL,
    [MeasurementType] [varchar](50) NULL,
    [AnatomicalFeature] [varchar](50) NULL,
    [Value] [float] NULL
) ON [PRIMARY]
