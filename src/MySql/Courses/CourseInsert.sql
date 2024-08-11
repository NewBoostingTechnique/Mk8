DROP procedure IF EXISTS `CourseInsert`;

CREATE PROCEDURE `CourseInsert` (
  IN Id BINARY(16),
  IN Name VARCHAR(255)
)
BEGIN
  INSERT INTO courses(
    Id,
    Name
  )
  VALUES(
    Id,
    Name
  );
END;
