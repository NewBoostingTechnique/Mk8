DROP procedure IF EXISTS `CourseList`;

DELIMITER $$
CREATE PROCEDURE `CourseList`(
)
BEGIN
  SELECT  courses.Name
  FROM    courses;
END$$

DELIMITER ;
