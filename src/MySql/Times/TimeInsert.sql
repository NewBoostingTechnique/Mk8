DROP procedure IF EXISTS `TimeInsert`;

CREATE PROCEDURE `TimeInsert` (
  IN CourseId BINARY(16),
  IN PlayerId BINARY(16),
  In TimeDate DATE,
  IN TimeId BINARY(16),
  IN TimeSpan TIME(3)
)
BEGIN
  INSERT INTO times(
    Id,
    CourseId,
    Date,
    PlayerId,
    TimeSpan
  )
  VALUES(
    TimeId,
    CourseId,
    TimeDate,
    PlayerId,
    TimeSpan
  );
END;
