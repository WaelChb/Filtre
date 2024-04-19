USE [WMS]
GO

/****** Object:  StoredProcedure [dbo].[GetOrders]    Script Date: 19/04/2024 09:38:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetOrders]
	-- Add the parameters for the stored procedure here
	@typeCommande VARCHAR(1), 
	@centrale VARCHAR(3),
	@typeArticle CHAR(1)
AS
BEGIN
	if @typeArticle = 'V' or @typeArticle = 'Verre'
	begin
		SELECT o.ORDERS,p.ARTICLE,p.NUMEROLIGNE,a.ArtikelKlasse_Id,p.typetransaction,N.QUANTITE AS QuantiteNom,N.ARTICLE AS ArticleNom,P.QUANTITE
		FROM dbo.ORDERS AS o
			INNER JOIN dbo.POSITIONS AS p ON p.ORDERS = o.ORDERS
			LEFT JOIN dbo.Nomenclatures AS n on n.Nomenclature = p.article
			LEFT JOIN verres.Artikel AS a ON a.ARTIKEL IN (p.ARTICLE, n.ARTICLE)
		WHERE o.ORDERS LIKE ''+@typeCommande+'%'
			AND ( o.CentraleX3 = @centrale or @centrale ='')
			AND p.ETAT = 1 --à verifier 
			AND o.ETAT = 1
			AND NOT EXISTS (
				SELECT * FROM dbo.POSITIONS AS p3
				WHERE p3.ORDERS = o.ORDERS AND ISNULL(p3.ARTICLE,'')  LIKE 'OPHRX%'
					
			)
				order by o.ORDERS DESC
				
		END;
		else
		begin
			SELECT o.ORDERS,p.ARTICLE,p.NUMEROLIGNE,a.ArtikelKlasse_Id,p.typetransaction,N.QUANTITE AS QuantiteNom,N.ARTICLE AS ArticleNom,P.QUANTITE
			FROM dbo.ORDERS AS o
				INNER JOIN dbo.POSITIONS AS p ON p.ORDERS = o.ORDERS
				LEFT JOIN dbo.Nomenclatures AS n on n.Nomenclature = p.article
				LEFT JOIN lentilles.Artikel AS a ON a.ARTIKEL IN (p.ARTICLE, n.ARTICLE)
			WHERE o.ORDERS LIKE ''+@typeCommande+'%'
				AND ( o.CentraleX3 = @centrale or @centrale ='')
				AND p.ETAT = 1 --à verifier 
				AND o.ETAT = 1
				AND NOT EXISTS (
					SELECT * FROM dbo.POSITIONS AS p3
					WHERE p3.ORDERS = o.ORDERS AND ISNULL(p3.ARTICLE,'')  LIKE 'OPHRX%'
						
				)
						order by o.ORDERS DESC

		END;
END;

GO


