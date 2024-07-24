DROP procedure IF EXISTS `CourseIdentify`;

CREATE PROCEDURE `CourseIdentify` (
  IN CourseName VARCHAR(255)
)
BEGIN
  SELECT
    courses.Id
  FROM
    courses
  WHERE
    courses.Name = CourseName;
END;
