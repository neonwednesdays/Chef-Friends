using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    card
*/
public class CardModel : BaseModel {
    // [SerializeField]
    public CardElement Element;

    public Vector3 Position; // where the view will end up
    
    // card data
    public string name;
    public int power;
    public string type;

    public CardModel() {
        
    }
}
