using System;
using WooCommerceNET;
using WooCommerceNET.WooCommerce.v2;

namespace StrumentiMusicali.WOOCommerceSyncro
{
    public class MasterClass
    {



        public void Test()
        {
            var rest = new RestAPI("http://provaale.atwebpages.com/wp-json/wc/v3/", "ck_8b2a242ed9688623d83efa63f21da8ae6b82736e", "cs_691cad866d216a0c85f3d56aacc0a55c92165a21");
            WCObject wc = new WCObject(rest);

            //Get all products
            var products = wc.Product.GetAll();
            products.Wait();
            var prodList = products.Result;
        }
    }
}
