
DROP procedure IF EXISTS `LoginInsert`;

CREATE PROCEDURE `LoginInsert` (
  IN Id BINARY(16),  
  IN Email VARCHAR(255),
  IN PersonId BINARY(16)
)
BEGIN
  INSERT INTO logins(
    Id,
    Email,
    PersonId
  )
  VALUES(
    Id,
    Email,
    PersonId
  );
END;
