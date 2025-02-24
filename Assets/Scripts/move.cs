using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//classes are isntructions
public class move : MonoBehaviour
{
    public int testInteger = 5;
    public int anotherInt = 10;
    public int thirdInt = 5;
    public string testString = "Hello World";
    public float testFloat = 3.14f;
    public bool testBool = true;

    // Start is called before the first frame update
    //methods 
    void Start()
    {
        Debug.Log(testString);

        if(testBool) {
            Debug.Log("the boolean is true");
        }else{
            Debug.Log("the boolean is false");
        }

        Debug.Log((testInteger)/thirdInt);
/*
        if(addNumbers(testInteger,anotherInt) > thirdInt){
            Debug.Log("the division of " + addNumbers(testInteger,anotherInt) + " is " + divideNumbers(addNumbers(testInteger, anotherInt),thirdInt));
        }else{
            Debug.Log(addNumbers(testInteger,anotherInt) + " is less than" + thirdInt);
        }
            */
    }


    // Update is called once per frame
    void Update()
    {
        
    }
void addstuff(){

}
    int addNumbers(int a, int b){
        int sum = a + b;
        Debug.Log(sum);
        return sum;
    }
        //2 ints, if greater than a third itn, than do a division

}
