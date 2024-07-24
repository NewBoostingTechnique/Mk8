DROP procedure IF EXISTS `ProofTypeIdentify`;

CREATE PROCEDURE `ProofTypeIdentify` (
  IN ProofTypeDescription VARCHAR(255)
)
BEGIN
  SELECT
    prooftypes.Id
  FROM
    prooftypes
  WHERE
    prooftypes.Description = ProofTypeDescription;
END;
