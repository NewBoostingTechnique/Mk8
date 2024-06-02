DROP procedure IF EXISTS `ProofTypeList`;

DELIMITER $$
CREATE PROCEDURE `ProofTypeList` ()
BEGIN
  SELECT
    prooftypes.Description
  FROM
    proofTypes;
END$$

DELIMITER ;
