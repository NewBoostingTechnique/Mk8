DROP procedure IF EXISTS `ProofTypeExists`;

DELIMITER $$
CREATE PROCEDURE `ProofTypeExists` (
  IN ProofTypeDescription VARCHAR(255)
)
BEGIN
  SELECT
    1
  FROM
    prooftypes
  WHERE
    prooftypes.Description = ProofTypeDescription;
END$$

DELIMITER ;
