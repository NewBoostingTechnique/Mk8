DROP procedure IF EXISTS `CourseExists`;

CREATE PROCEDURE `CourseExists`(
  IN CourseName VARCHAR(255)
)
BEGIN
  SELECT  1
  FROM    courses
  WHERE   Name = CourseName;
END;