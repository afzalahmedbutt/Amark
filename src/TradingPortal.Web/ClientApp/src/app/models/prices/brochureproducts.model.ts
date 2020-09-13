
export class Brochure_Products {
  /*[Key]*/
  public Brochure_Product_Id: number;
  public Name: string;
  public Type: string;
  public Section: string;
  public Fineness: string;
  public Image_Path: string;
  public Back_Image_Path: string;
  public Sort_Order: number;
  public Description: string;
  public Is_Active: string;
  public Brochure_Product_Variants: Brochure_Product_Variants[];
  //public Products: List<Product>;
}


export class Brochure_Product_Variants {
  /*[Key]*/
  public Brochure_Product_Variant_Id: number;
  public Brochure_Product_Id: number;
  public Size: string;
  public Diameter: string;
  public Thickness: string;
  public Dimensions: string;
  public Weight: number;
  //public Brochure_Products: Brochure_Products;
}
