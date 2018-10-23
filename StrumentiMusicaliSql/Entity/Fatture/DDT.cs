using StrumentiMusicali.Library.Entity.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrumentiMusicali.Library.Entity
{
	public class DDt : DocumentoFiscaleBase
	{
		 public	DDt()
			: base()
		{
			TipoDocumento = 1;
		}
	}
}