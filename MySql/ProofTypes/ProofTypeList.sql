DROP procedure IF EXISTS `ProofTypeList`;

CREATE PROCEDURE `ProofTypeList` ()
BEGIN
  SELECT
    prooftypes.Description
  FROM
    proofTypes;
END;
