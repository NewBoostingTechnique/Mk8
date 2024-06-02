DROP procedure IF EXISTS `CourseExists`;

DELIMITER $$
CREATE PROCEDURE `CourseExists`(
  IN CourseName VARCHAR(255)
)
BEGIN
  SELECT  1
  FROM    courses
  WHERE   Name = CourseName;
END$$

DELIMITER ;
