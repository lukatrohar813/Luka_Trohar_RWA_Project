
go
BEGIN TRANSACTION

BEGIN TRY

--Type 1-to-N
CREATE TABLE [dbo].[Type] (
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(256) NOT NULL,
    [Description] NVARCHAR(MAX),
	CONSTRAINT [UQ_Type_Name] UNIQUE ([Name]),
	CONSTRAINT [PK_Type] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    )
);


--Image 1-to-M
CREATE TABLE [dbo].[Image] (
	[Id][int] IDENTITY (1,1) NOT NULL,
	[ContentType] NVARCHAR(MAX) NOT NULL,
	[FileName] NVARCHAR(1000) NOT NULL,
	[FilePath] NVARCHAR(MAX)NOT NULL,
	CONSTRAINT [UQ_Image_FileName] UNIQUE ([FileName]),
	CONSTRAINT [PK_Image] PRIMARY KEY CLUSTERED 
	(
	[ID] ASC
	)
);

--Primary 
CREATE TABLE [dbo].[Project] (
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(256) NOT NULL,
    [Description] NVARCHAR(MAX),
    [TypeId] INT NOT NULL,
    [StartDate] DATE NOT NULL,
    [EndDate] DATE NOT NULL,
	[ImageId] INT,
	CONSTRAINT [UQ_Project_Name] UNIQUE ([Name]),
    CONSTRAINT [FK_Project_Image] FOREIGN KEY ([ImageId]) REFERENCES [dbo].[Image]([Id])ON DELETE SET NULL,
	CONSTRAINT [FK_Project_Type] FOREIGN KEY ([TypeId]) REFERENCES [dbo].[Type]([Id])ON DELETE CASCADE,
	CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    )
);

-- Skill-M-to-N
CREATE TABLE [dbo].[Skill] (
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(256) NOT NULL,
    [Description] NVARCHAR(MAX),
	CONSTRAINT [UQ_Skill_Name] UNIQUE ([Name]),
	CONSTRAINT [PK_Skill] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    )
);

--ProjectSkill-M-to-N-bridge
CREATE TABLE [dbo].ProjectSkill (
    [Id] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [int] NOT NULL,
	[SkillId] [int] NOT NULL,
	CONSTRAINT [FK_ProjectSkill_Project] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ProjectSkill_Skill] FOREIGN KEY ([SkillId]) REFERENCES [dbo].[Skill]([Id])ON DELETE CASCADE,
    CONSTRAINT [PK_ProjectSkill] PRIMARY KEY CLUSTERED 
	 (
        [Id] ASC
    )
);

-- User M-to-N
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL DEFAULT (getutcdate()),
	[DeletedAt] [datetime2](7) NULL,
	[Username] [nvarchar](50) NOT NULL,
	[FirstName] [nvarchar](256) NOT NULL,
	[LastName] [nvarchar](256) NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[PwdHash] [nvarchar](256) NOT NULL,
	[PwdSalt] [nvarchar](256) NOT NULL,
	[Phone] [nvarchar](256) NULL,
	[Role] [int] NOT NULL,
	[SecurityToken] [nvarchar](256) NOT NULL,
	[IsDeleted] BIT NOT NULL DEFAULT(0),
	CONSTRAINT [UQ_User_Username] UNIQUE ([Username]),
    CONSTRAINT [UQ_User_Email] UNIQUE ([Email]),
	CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)

-- User/Project-M-to-N-bridge
CREATE TABLE [dbo].[Application] (
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [UserId] INT NOT NULL,
    [ProjectId] INT NOT NULL,
    [CreatedAt] DATE NOT NULL DEFAULT (getutcdate()),
    [Status] NVARCHAR(50) NOT NULL DEFAULT 'Pending',
    CONSTRAINT [FK_Application_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([Id])ON DELETE CASCADE,
    CONSTRAINT [FK_Application_Project] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project]([Id])ON DELETE CASCADE,
    CONSTRAINT [PK_Application] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    )
);

--Log 1-to-m
CREATE TABLE [dbo].[Log](
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Message] NVARCHAR(MAX) NOT NULL,
    [Level] NVARCHAR(50) NOT NULL,
    [Timestamp] DATETIME NOT NULL DEFAULT (GETUTCDATE()),
    CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    )
);





CREATE NONCLUSTERED INDEX idx_Project_TypeId ON [dbo].[Project]([TypeId]);
CREATE NONCLUSTERED INDEX idx_ProjectSkill_ProjectId ON [dbo].[ProjectSkill]([ProjectId]);
CREATE NONCLUSTERED INDEX idx_ProjectSkill_SkillId ON [dbo].[ProjectSkill]([SkillId]);
CREATE NONCLUSTERED INDEX idx_User_Username ON [dbo].[User]([Username]);
CREATE NONCLUSTERED INDEX idx_User_Email ON [dbo].[User]([Email]);
CREATE NONCLUSTERED INDEX idx_Application_UserId ON [dbo].[Application]([UserId]);
CREATE NONCLUSTERED INDEX idx_Application_ProjectId ON [dbo].[Application]([ProjectId]);



COMMIT TRANSACTION

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO


--use VOLUNTEER_PLATFORM
BEGIN TRANSACTION

BEGIN TRY

INSERT INTO [dbo].[Skill] ([Name], [Description])
VALUES
    ('Event Planning', 'Organizing and coordinating events and activities.'),
    ('Tutoring', 'Providing academic tutoring and educational support.'),
    ('Fundraising', 'Planning and executing fundraising campaigns and events.'),
    ('Public Speaking', 'Effective communication through public speaking.'),
    ('Project Management', 'Planning, organizing, and managing projects.'),
    ('Counseling', 'Providing emotional support and guidance to individuals.'),
    ('First Aid', 'Administering basic first aid and emergency medical care.'),
    ('Digital Literacy', 'Teaching digital skills and technology literacy.'),
    ('Gardening', 'Cultivating and maintaining gardens and green spaces.'),
    ('Art Therapy', 'Using artistic expression for therapeutic purposes.');

INSERT INTO [dbo].[Type] ([Name], [Description])
VALUES
    ('Environmental', 'Projects focused on environmental conservation and sustainability.'),
    ('Education', 'Projects aimed at promoting education and literacy.'),
    ('Community Service', 'Projects involving community service and volunteerism.'),
    ('Healthcare', 'Projects related to healthcare and public health.'),
    ('Social Services', 'Projects providing social services to vulnerable populations.'),
    ('Arts and Culture', 'Projects promoting arts, culture, and creative expression.'),
    ('Sports and Recreation', 'Projects focused on sports, fitness, and recreational activities.'),
    ('Technology', 'Projects involving technology innovation and digital skills.'),
    ('Animal Welfare', 'Projects dedicated to the welfare and protection of animals.'),
    ('Emergency Response', 'Projects focused on disaster preparedness and emergency response.');

INSERT INTO [dbo].[Image] (ContentType, FileName, FilePath) VALUES
('image/jpeg','default.jpeg', '/images/default.jpeg')

INSERT INTO [dbo].[Project] ([Name], [Description], [TypeId], [StartDate], [EndDate], [ImageId])
VALUES
    ('Community Clean-up Campaign', 'Organizing a community-wide clean-up event to improve local environment.', 1, '2024-08-15', '2024-08-16', 1),
    ('Youth Tutoring Program', 'Providing academic tutoring and mentoring for underprivileged youth.', 2, '2024-09-01', '2024-10-15', 1),
    ('Elderly Companionship Initiative', 'Visiting and providing companionship to elderly residents in nursing homes.', 3, '2024-07-20', '2024-08-10', 1),
    ('Food Drive for Homeless Shelters', 'Collecting and distributing food donations to local homeless shelters.', 4, '2024-08-01', '2024-08-31', 1),
    ('Environmental Education Workshop', 'Hosting workshops to educate the community on environmental conservation.', 1, '2024-09-10', '2024-09-12', 1),
    ('Healthcare Awareness Campaign', 'Raising awareness about healthcare issues through community events.', 2, '2024-07-25', '2024-07-28', 1),
    ('Animal Shelter Volunteer Day', 'Volunteering at local animal shelters to care for abandoned animals.', 3, '2024-08-05', '2024-08-06', 1),
    ('Youth Sports Coaching Program', 'Coaching and mentoring youth in various sports activities.', 4, '2024-08-03', '2024-09-20', 1),
    ('Digital Literacy Training', 'Teaching digital skills to seniors to improve their technology literacy.', 1, '2024-09-15', '2024-09-30', 1),
    ('Community Garden Project', 'Establishing and maintaining community gardens for sustainable food production.', 2, '2024-07-30', '2024-09-10', 1),
    ('Art Therapy Workshops', 'Conducting art therapy sessions for individuals coping with mental health issues.', 3, '2024-08-12', '2024-08-14', 1),
    ('Emergency Response Training', 'Providing training sessions on emergency response and disaster preparedness.', 4, '2024-09-05', '2024-09-07', 1),
    ('STEM Education Outreach', 'Promoting STEM education among underserved youth through interactive workshops.', 1, '2024-08-08', '2024-08-10', 1),
    ('Cultural Exchange Festival', 'Organizing a festival to celebrate cultural diversity and promote cross-cultural understanding.', 2, '2024-07-15', '2024-07-17', 1),
    ('Music Therapy Sessions', 'Offering music therapy sessions for individuals with special needs and disabilities.', 3, '2024-09-01', '2024-09-30', 1),
    ('Community Library Renovation', 'Renovating and modernizing local community libraries to enhance public access to resources.', 4, '2024-08-20', '2024-09-30', 1),
    ('Disaster Relief Fundraising', 'Organizing fundraisers to support disaster relief efforts in affected communities.', 1, '2024-07-20', '2024-08-05', 1),
    ('Youth Empowerment Conference', 'Hosting a conference to empower and inspire youth to achieve their goals.', 2, '2024-08-25', '2024-08-27', 1),
    ('Healthcare Outreach Clinic', 'Setting up temporary healthcare clinics to provide medical services to underserved populations.', 3, '2024-09-10', '2024-09-12', 1),
    ('Volunteer Appreciation Event', 'Celebrating and recognizing the contributions of volunteers in the community.', 4, '2024-07-30', '2024-07-31', 1);


	INSERT INTO [dbo].[User] ([CreatedAt], [Username], [FirstName], [LastName], [Email], [PwdHash], [PwdSalt], [Phone], [Role], [SecurityToken], [IsDeleted])
VALUES
    (GETUTCDATE(), 'volunteer1', 'Alice', 'Smith', 'alice.smith@example.com', 'T3yEj9exowKmyucIF12A19sg3O4EWdzHWDf2luUGIUM=', 'DcypE+1XDSiWo5rLCLzpmw==', NULL, 1, 'token1', 0),
    (GETUTCDATE(), 'volunteer2', 'Bob', 'Johnson', 'bob.johnson@example.com', 'T3yEj9exowKmyucIF12A19sg3O4EWdzHWDf2luUGIUM=', 'DcypE+1XDSiWo5rLCLzpmw==', NULL, 1, 'token2', 0),
    (GETUTCDATE(), 'volunteer3', 'Carol', 'Williams', 'carol.williams@example.com', 'T3yEj9exowKmyucIF12A19sg3O4EWdzHWDf2luUGIUM=', 'DcypE+1XDSiWo5rLCLzpmw==', NULL, 1, 'token3', 0),
    (GETUTCDATE(), 'volunteer4', 'David', 'Brown', 'david.brown@example.com', 'T3yEj9exowKmyucIF12A19sg3O4EWdzHWDf2luUGIUM=', 'DcypE+1XDSiWo5rLCLzpmw==', NULL, 1, 'token4', 0),
    (GETUTCDATE(), 'volunteer5', 'Emma', 'Davis', 'emma.davis@example.com', 'T3yEj9exowKmyucIF12A19sg3O4EWdzHWDf2luUGIUM=', 'DcypE+1XDSiWo5rLCLzpmw==', NULL, 1, 'token5', 0),
    (GETUTCDATE(), 'volunteer6', 'Frank', 'Miller', 'frank.miller@example.com', 'T3yEj9exowKmyucIF12A19sg3O4EWdzHWDf2luUGIUM=', 'DcypE+1XDSiWo5rLCLzpmw==', NULL, 1, 'token6', 0),
    (GETUTCDATE(), 'volunteer7', 'Grace', 'Wilson', 'grace.wilson@example.com', 'T3yEj9exowKmyucIF12A19sg3O4EWdzHWDf2luUGIUM=', 'DcypE+1XDSiWo5rLCLzpmw==', NULL, 1, 'token7', 0),
    (GETUTCDATE(), 'volunteer8', 'Henry', 'Moore', 'henry.moore@example.com', 'T3yEj9exowKmyucIF12A19sg3O4EWdzHWDf2luUGIUM=', 'DcypE+1XDSiWo5rLCLzpmw==', NULL, 1, 'token8', 0),
    (GETUTCDATE(), 'volunteer9', 'Ivy', 'Anderson', 'ivy.anderson@example.com', 'T3yEj9exowKmyucIF12A19sg3O4EWdzHWDf2luUGIUM=', 'DcypE+1XDSiWo5rLCLzpmw==', NULL, 1, 'token9', 0),
    (GETUTCDATE(), 'volunteer10', 'Jack', 'Taylor', 'jack.taylor@example.com', 'T3yEj9exowKmyucIF12A19sg3O4EWdzHWDf2luUGIUM=', 'DcypE+1XDSiWo5rLCLzpmw==', NULL, 1, 'token10', 0);

	INSERT INTO [dbo].[Application] ([UserId], [ProjectId], [CreatedAt], [Status])
VALUES
    (4, 4, GETUTCDATE(), 'Approved'),
    (2, 2, GETUTCDATE(), 'Pending'),
    (3, 3, GETUTCDATE(), 'Pending'),
    (4, 4, GETUTCDATE(), 'Approved'),
    (5, 5, GETUTCDATE(), 'Pending'),
    (6, 6, GETUTCDATE(), 'Approved'),
    (7, 7, GETUTCDATE(), 'Pending'),
    (8, 8, GETUTCDATE(), 'Pending'),
    (9, 9, GETUTCDATE(), 'Approved'),
    (10, 10, GETUTCDATE(), 'Pending');




COMMIT TRANSACTION

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO

