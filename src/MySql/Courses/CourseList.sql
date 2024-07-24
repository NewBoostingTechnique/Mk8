DROP procedure IF EXISTS `CourseList`;

CREATE PROCEDURE `CourseList`(
)
BEGIN
  SELECT  courses.Name
  FROM    courses;
END;
