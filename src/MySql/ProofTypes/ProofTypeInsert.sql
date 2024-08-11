
DROP procedure IF EXISTS `ProofTypeInsert`;

CREATE PROCEDURE `ProofTypeInsert` (
  IN Id BINARY(16),
  IN Description VARCHAR(255)
)
BEGIN
  INSERT INTO prooftypes(
    Id,
    Description
  )
  VALUES(
    Id,
    Description
  );
END;
