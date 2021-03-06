#region License
/* **********************************************************************************
 * Copyright (c) Roman Ivantsov
 * This source code is subject to terms and conditions of the MIT License
 * for Irony. A copy of the license can be found in the License.txt file
 * at the root of this distribution. 
 * By using this source code in any fashion, you are agreeing to be bound by the terms of the 
 * MIT License.
 * You must not remove this notice from this software.
 * **********************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using Irony.Interpreter;
using Irony.Parsing;

namespace Irony.Interpreter.Ast {
  public class IfNode : AstNode {
    public AstNode Test;
    public AstNode IfTrue;
    public AstNode IfFalse;

    public IfNode() { }

    public override void Init(ParsingContext context, ParseTreeNode treeNode) {
      base.Init(context, treeNode);
      Test = AddChild("Test", treeNode.MappedChildNodes[0]);
      IfTrue = AddChild("IfTrue", treeNode.MappedChildNodes[1]);
      if (treeNode.MappedChildNodes.Count > 2)
        IfFalse = AddChild("IfFalse", treeNode.MappedChildNodes[2]);
    }

    protected override object DoEvaluate(ScriptThread thread) {
      thread.CurrentNode = this;  //standard prolog
      object result = null; 
      var test = Test.Evaluate(thread);
      var isTrue = thread.Runtime.IsTrue(test);
      if (isTrue) {
        if (IfTrue != null)
          result = IfTrue.Evaluate(thread);
      } else {
        if (IfFalse != null)
          result = IfFalse.Evaluate(thread);
      }
      thread.CurrentNode = Parent; //standard epilog
      return result; 
    }

    public override void SetIsTail() {
      base.SetIsTail();
      if (IfTrue != null)
        IfTrue.SetIsTail();
      if (IfFalse != null)
        IfFalse.SetIsTail(); 
    }

  }//class

}//namespace
