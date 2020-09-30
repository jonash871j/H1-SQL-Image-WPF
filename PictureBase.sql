-- DATABASE CREATION
CREATE DATABASE PictureBase
-------------------------------------------------------------------------------

-- TABLE CREATIONS
CREATE TABLE Picture (
	id int identity(1,1) primary key,
	p_name varchar(255),
	p_binary varbinary(MAX)
)
-------------------------------------------------------------------------------

-- INSERT PICTURE
CREATE PROCEDURE InsertPicture @name varchar(255), @pbinary varbinary(MAX)
AS

INSERT INTO Picture(p_name, p_binary)
VALUES(@name, @pbinary)

GO
-------------------------------------------------------------------------------

-- DELETE PICTURE
CREATE PROCEDURE DeletePicture @id int
AS

DELETE FROM Picture
WHERE id = @id

GO
-------------------------------------------------------------------------------

-- SELECT ALL PICTURES
CREATE PROCEDURE SelectPicture
AS

SELECT *
FROM Picture

GO
-------------------------------------------------------------------------------