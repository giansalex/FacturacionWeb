//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.3.0.0 (entitiestodtos.codeplex.com).
//     Timestamp: 2016/07/25 - 09:20:17
//
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//-------------------------------------------------------------------------------------------------------


namespace WebDataModel.Entities
{
    public partial class datahierarchyDto
    {
        
        public int i_GroupId { get; set; }

        
        public int i_ItemId { get; set; }

        
        public string v_Value1 { get; set; }

        
        public string v_Value2 { get; set; }

        
        public string v_Field { get; set; }

        
        public int? i_ParentItemId { get; set; }

        
        public int? i_Header { get; set; }

        
        public int? i_Sort { get; set; }

        
        public int? i_IsDeleted { get; set; }

        public datahierarchyDto()
        {
        }

        public datahierarchyDto(int i_GroupId, int i_ItemId, string v_Value1, string v_Value2, string v_Field, int? i_ParentItemId, int? i_Header, int? i_Sort, int? i_IsDeleted)
        {
			this.i_GroupId = i_GroupId;
			this.i_ItemId = i_ItemId;
			this.v_Value1 = v_Value1;
			this.v_Value2 = v_Value2;
			this.v_Field = v_Field;
			this.i_ParentItemId = i_ParentItemId;
			this.i_Header = i_Header;
			this.i_Sort = i_Sort;
			this.i_IsDeleted = i_IsDeleted;
        }
    }
}