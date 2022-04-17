

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace ForresterModeller.src.ProjectManager.MiniParser
{



    class Lexem
    {
      public  Lexem(int n, string s)
        { first = n;
            second = s;
        }
       public int first;
        public string second;

    };
    public class MinParser
    {

        const int tId = 1;
        const int tNumb = 2;
        const int tPlus = 41;
        const int tMinus = 42;
        const int tMult = 43;
        const int tDiv = 44;
        const int tStep = 45;
        const int tSave = 46;
        const int tEnd = 100;


        String formul;
        string text;

        public void check(string t)
        { text = t + '\0'; }

        List<Lexem> allLex= new();

        int index = 0;
        private String getNext()
        {
            return "";
        }


        int pointer;
        int get_pointer() {
    return pointer;
}

    void put_pointer(int pointer)
    {
        pointer = pointer;
    }


        string scanNumb()
        {
            string value = "";
            value += text[pointer++];
            while (Char.IsDigit(text[pointer]))
                value += text[pointer++];
            return value;
        }
    private Lexem cutForm()
        { string value="";
            pointer = 0;
            while (true)
            {
                while (text[pointer] == ' ') 
                {
                    pointer++;
                }

                if (Char.IsDigit(text[pointer]))
                {

                    value = scanNumb();
                    if (text[pointer] == '.' && Char.IsDigit(text[pointer+1]))
                    { pointer++; value += (text[pointer] + scanNumb()); }
                    return new Lexem(tNumb, value);
                }



            }

        }

        public MinParser()
        { }

        /*



        DataTypeAndValue  expressionAnalysis()
        {////////////////////////////////////////////////////////////////////////////
            DataTypeAndValue type = logI();
            Lexem lex = getCurrentLexeme();

            while (lex.first == tOr)
            {
                this->pointer++;
                DataTypeAndValue typeDop = logI();
                TypeVar t = root->semResType(type.type, typeDop.type);
                type = perevod(t, type);
                typeDop = perevod(t, typeDop);

                if (t == TTBool)
                {
                    type.data.Data_bool = type.data.Data_bool || typeDop.data.Data_bool;
                }
                else if (t == TTInt)
                {
                    type.data.Data_bool = type.data.Data_int || typeDop.data.Data_int;



                }
                type.type = TTBool;
                interCode->addTriad(tOr, type.oper, typeDop.oper);//*****************************************************************************************************    
                type.oper = Oper{ true, interCode->getK() - 1, Lexem{ 1, "", 1} };//***********************************************************************************************************


                lex = getCurrentLexeme();
            }
            return type;
        }

        DataTypeAndValue  logI()
        {////////////////////////////////////////////////////////////////////////////
            DataTypeAndValue type = eqFunc1();
            Lexem lex = getCurrentLexeme();

            while (lex.first == tAnd)
            {
                this->pointer++;

                DataTypeAndValue typeDop = eqFunc1();
                TypeVar t = root->semResType(type.type, typeDop.type);
                type = perevod(t, type);
                typeDop = perevod(t, typeDop);

                if (t == TTBool)
                {
                    type.data.Data_bool = type.data.Data_bool && typeDop.data.Data_bool;

                }
                else if (t == TTInt)
                {
                    type.data.Data_bool = type.data.Data_int && typeDop.data.Data_int;


                }
                type.type = TTBool;

                interCode->addTriad(tAnd, type.oper, typeDop.oper);//*****************************************************************************************************    
                type.oper = Oper{ true, interCode->getK() - 1, Lexem{ 1, "", 1} };//***********************************************************************************************************


                lex = getCurrentLexeme();
            }
            return type;
        }

        DataTypeAndValue  eqFunc1()
        {////////////////////////////////////////////////////////////////////////////
            DataTypeAndValue type = eqFunc2();
            Lexem lex = getCurrentLexeme();

            while (lex.first == tEq || lex.first == tNeq)
            {

                this->pointer++;

                DataTypeAndValue typeDop = eqFunc2();
                TypeVar t = root->semResType(type.type, typeDop.type);
                type = perevod(t, type);
                typeDop = perevod(t, typeDop);

                if (t == TTBool)
                {
                    type.data.Data_bool = type.data.Data_bool == typeDop.data.Data_bool;
                }

                else if (t == TTInt)
                {
                    type.data.Data_bool = type.data.Data_int == typeDop.data.Data_int;
                }
                type.type = TTBool;
                if (lex.first == tNeq)
                {
                    type.data.Data_bool = !type.data.Data_bool;

                    interCode->addTriad(tNeq, type.oper, typeDop.oper);//*****************************************************************************************************    
                }
                else
                    interCode->addTriad(tEq, type.oper, typeDop.oper);


                type.oper = Oper{ true, interCode->getK() - 1, Lexem{ 1, "", 1} };//***********************************************************************************************************

                lex = getCurrentLexeme();
            }
            return type;
        }

        DataTypeAndValue  eqFunc2()
        {////////////////////////////////////////////////////////////////////////////
            DataTypeAndValue type = eqFunc3();
            Lexem lex = getCurrentLexeme();

            while (lex.first == tMore || lex.first == tLe)
            {
                this->pointer++;

                DataTypeAndValue typeDop = eqFunc3();
                TypeVar t = root->semResType(type.type, typeDop.type);
                type = perevod(t, type);
                typeDop = perevod(t, typeDop);

                if (t == TTBool)
                {
                    type.data.Data_bool = type.data.Data_bool <= typeDop.data.Data_bool;
                }

                else if (t == TTInt)
                {
                    type.data.Data_bool = type.data.Data_int <= typeDop.data.Data_int;
                }
                type.type = TTBool;
                if (lex.first == tMore)
                {
                    type.data.Data_bool = !type.data.Data_bool;
                    interCode->addTriad(tMore, type.oper, typeDop.oper);//*****************************************************************************************************    
                }
                else
                    interCode->addTriad(tLe, type.oper, typeDop.oper);


                type.oper = Oper{ true, interCode->getK() - 1, Lexem{ 1, "", 1} };//***********************************************************************************************************

                lex = getCurrentLexeme();
            }
            return type;
        }

        DataTypeAndValue  eqFunc3()
        {////////////////////////////////////////////////////////////////////////////
            DataTypeAndValue type = add();
            Lexem lex = getCurrentLexeme();

            while (lex.first == tLess ||
                lex.first == tMe)
            {
                this->pointer++;
                DataTypeAndValue typeDop = add();
                TypeVar t = root->semResType(type.type, typeDop.type);
                type = perevod(t, type);
                typeDop = perevod(t, typeDop);
                if (t == TTBool)
                {
                    type.data.Data_bool = type.data.Data_bool >= typeDop.data.Data_bool;
                }

                else if (t == TTInt)
                {
                    type.data.Data_bool = type.data.Data_int >= typeDop.data.Data_int;
                }
                type.type = TTBool;
                if (lex.first == tLess)
                {
                    type.data.Data_bool = !type.data.Data_bool;
                    interCode->addTriad(tLess, type.oper, typeDop.oper);//*****************************************************************************************************    
                }
                else
                    interCode->addTriad(tMe, type.oper, typeDop.oper);


                type.oper = Oper{ true, interCode->getK() - 1, Lexem{ 1, "", 1} };//***********************************************************************************************************

                lex = getCurrentLexeme();
            }
            return type;
        }


        DataTypeAndValue  add()
        {
            DataTypeAndValue type = multiplier();
            Lexem lex = getCurrentLexeme();
            while (lex.first == tPlus || lex.first == tMinus)
            {
                this->pointer++;
                DataTypeAndValue typeDop = multiplier();
                TypeVar t = root->semResType(type.type, typeDop.type);
                type = perevod(t, type);
                typeDop = perevod(t, typeDop);
                if (lex.first == tPlus)
                {
                    if (t == TTBool)
                    {
                        type.data.Data_int = type.data.Data_bool + typeDop.data.Data_bool;
                    }

                    else if (t == TTInt)
                    {
                        type.data.Data_int = type.data.Data_int + typeDop.data.Data_int;
                    }
                    interCode->addTriad(tPlus, type.oper, typeDop.oper);//*****************************************************************************************************         

                }

                else if (lex.first == tMinus)
                {
                    if (t == TTBool)
                    {
                        type.data.Data_int = type.data.Data_bool - typeDop.data.Data_bool;
                    }

                    else if (t == TTInt)
                    {
                        type.data.Data_int = type.data.Data_int - typeDop.data.Data_int;
                    }
                    interCode->addTriad(tMinus, type.oper, typeDop.oper);//*****************************************************************************************************         

                }


                type.type = TTInt;


                type.oper = Oper{ true, interCode->getK() - 1, Lexem{ 1, "", 1} };//***********************************************************************************************************

                lex = getCurrentLexeme();
            }
            return type;
        }

        DataTypeAndValue  multiplier()
        {
            DataTypeAndValue type = logNe();
            Lexem lex = getCurrentLexeme();


            while (lex.first == tMult || lex.first == tDiv)
            {
                this->pointer++;
                DataTypeAndValue typeDop = logNe();
                TypeVar t = root->semResType(type.type, typeDop.type);
                type = perevod(t, type);
                typeDop = perevod(t, typeDop);
                if (lex.first == tMult)
                {
                    if (t == TTBool)
                    {
                        type.data.Data_int = type.data.Data_bool * typeDop.data.Data_bool;

                    }
                    else if (t == TTInt)
                    {
                        type.data.Data_int = type.data.Data_int * typeDop.data.Data_int;
                    }
                    interCode->addTriad(tMult, type.oper, typeDop.oper);//*****************************************************************************************************  
                }

                else if (lex.first == tDiv)
                {
                    if (t == TTBool)
                    {
                        type.data.Data_int = type.data.Data_bool / typeDop.data.Data_bool;
                    }

                    else if (t == TTInt)
                    {
                        type.data.Data_int = type.data.Data_int / typeDop.data.Data_int;
                    }
                    interCode->addTriad(tDiv, type.oper, typeDop.oper);//*****************************************************************************************************  
                }


                type.type = TTInt;


                type.oper = Oper{ true, interCode->getK() - 1, Lexem{ 1, "", 1} };//***********************************************************************************************************

                lex = getCurrentLexeme();
            }
            return type;
        }


        DataTypeAndValue  logNe()
        {
            DataTypeAndValue type = { 0, TTypeDef };
            Lexem lex = getCurrentLexeme();
            if (lex.first == tNot)
            {


                this->pointer++;
                type = elementaryExpressionAnalysis();




                if (type.type == TTBool)
                    type.data.Data_bool = !type.data.Data_bool;
                else
                {
                    type.data.Data_bool = !type.data.Data_int;

                }

                type.type = TTBool;

                interCode->addTriad(tNot, type.oper, Oper{ false, 0, Lexem{ 1, "", 1} });//*****************************************************************************************************         
                type.oper = Oper{ true, interCode->getK() - 1, Lexem{ 1, "", 1} };//***********************************************************************************************************

            }
            else
                type = elementaryExpressionAnalysis();

            return type;
        }






        bool elementaryExpressionAnalysis()
        {

            DataTypeAndValue type = { 0, TTypeDef };


            Lexem lex = getCurrentLexeme();
            //константа
            if (lex.first == constInt || lex.first == constHex || lex.first == tTrue || lex.first == tFalse)
            {



                type.type = root->FromConstToType(lex.first);
                if (type.type == TTBool)
                    type.data.Data_bool = (lex.first == tTrue) ? true : false;
                else
                    if (type.type == TTInt)
                    type.data.Data_int = stoi(lex.second);

                type.oper = Oper{ false, 0, lex};
                this->pointer++;

            }
            //идентификатор
            else if (lex.first == tId)
            {


                //SEMANTIC>>>>>>>>>>>>>>>>>>>



                //вызов функции
                if (lexemes[pointer + 1].first == tLs)
                {
                    type = CallFunction();
                    //typeObject = root->SemGetTypeF(lexemes[pointer - 1]);

                }//
                else
                {
                    lex = getNextLexeme();
                    //  typeObject = root->SemGetTypeV(lexemes[pointer - 1]);

                    type = root->GetValueIden(lexemes[pointer - 1]);
                    type.oper = Oper{ false, 0, lexemes[pointer - 1] };//*****************************************


                }
            }
            //(выражение)
            else if (lex.first == tLs)
            {
                pointer++;
                type = expressionAnalysis();


                lex = getCurrentLexeme();
                if (lex.first != tPs) showError("Error, expected: ')'!", lex);
                pointer++;
            }//
            else showError("Error, expected: Value", lex);

            return type;
        }


        */



    }
}
