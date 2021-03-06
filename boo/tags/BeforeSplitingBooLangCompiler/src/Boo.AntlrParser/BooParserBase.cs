// $ANTLR 2.7.3rc2: "src/Boo.AntlrParser/boo.g" -> "BooParserBase.cs"$

namespace Boo.AntlrParser
{
	// Generate the header common to all output files.
	using System;
	
	using TokenBuffer              = antlr.TokenBuffer;
	using TokenStreamException     = antlr.TokenStreamException;
	using TokenStreamIOException   = antlr.TokenStreamIOException;
	using ANTLRException           = antlr.ANTLRException;
	using LLkParser = antlr.LLkParser;
	using Token                    = antlr.Token;
	using TokenStream              = antlr.TokenStream;
	using RecognitionException     = antlr.RecognitionException;
	using NoViableAltException     = antlr.NoViableAltException;
	using MismatchedTokenException = antlr.MismatchedTokenException;
	using SemanticException        = antlr.SemanticException;
	using ParserSharedInputState   = antlr.ParserSharedInputState;
	using BitSet                   = antlr.collections.impl.BitSet;
	
using Boo.Lang.Ast;
using Boo.AntlrParser.Util;

public delegate void ParserErrorHandler(antlr.RecognitionException x);

	public 	class BooParserBase : antlr.LLkParser
	{
		public const int EOF = 1;
		public const int NULL_TREE_LOOKAHEAD = 3;
		public const int TIMESPAN = 4;
		public const int DOUBLE = 5;
		public const int LONG = 6;
		public const int ESEPARATOR = 7;
		public const int INDENT = 8;
		public const int DEDENT = 9;
		public const int COMPILATION_UNIT = 10;
		public const int PARAMETERS = 11;
		public const int PARAMETER = 12;
		public const int ELIST = 13;
		public const int DLIST = 14;
		public const int TYPE = 15;
		public const int CALL = 16;
		public const int STMT = 17;
		public const int BLOCK = 18;
		public const int FIELD = 19;
		public const int MODIFIERS = 20;
		public const int MODULE = 21;
		public const int LITERAL = 22;
		public const int LIST_LITERAL = 23;
		public const int UNPACKING = 24;
		public const int ABSTRACT = 25;
		public const int AND = 26;
		public const int AS = 27;
		public const int BREAK = 28;
		public const int CONTINUE = 29;
		public const int CAST = 30;
		public const int CLASS = 31;
		public const int CONSTRUCTOR = 32;
		public const int DEF = 33;
		public const int ELSE = 34;
		public const int ENSURE = 35;
		public const int ENUM = 36;
		public const int EXCEPT = 37;
		public const int FAILURE = 38;
		public const int FINAL = 39;
		public const int FROM = 40;
		public const int FOR = 41;
		public const int FALSE = 42;
		public const int GET = 43;
		public const int GIVEN = 44;
		public const int IMPORT = 45;
		public const int INTERFACE = 46;
		public const int INTERNAL = 47;
		public const int IS = 48;
		public const int ISA = 49;
		public const int IF = 50;
		public const int IN = 51;
		public const int NOT = 52;
		public const int NULL = 53;
		public const int OR = 54;
		public const int OTHERWISE = 55;
		public const int OVERRIDE = 56;
		public const int PASS = 57;
		public const int NAMESPACE = 58;
		public const int PUBLIC = 59;
		public const int PROTECTED = 60;
		public const int PRIVATE = 61;
		public const int RAISE = 62;
		public const int RETURN = 63;
		public const int RETRY = 64;
		public const int SET = 65;
		public const int SELF = 66;
		public const int SUPER = 67;
		public const int STATIC = 68;
		public const int SUCCESS = 69;
		public const int TRY = 70;
		public const int TRANSIENT = 71;
		public const int TRUE = 72;
		public const int TYPEOF = 73;
		public const int UNLESS = 74;
		public const int VIRTUAL = 75;
		public const int WHEN = 76;
		public const int WHILE = 77;
		public const int YIELD = 78;
		public const int EOS = 79;
		public const int TRIPLE_QUOTED_STRING = 80;
		public const int ID = 81;
		public const int ASSIGN = 82;
		public const int LBRACK = 83;
		public const int COMMA = 84;
		public const int RBRACK = 85;
		public const int LPAREN = 86;
		public const int RPAREN = 87;
		public const int COLON = 88;
		public const int QMARK = 89;
		public const int CMP_OPERATOR = 90;
		public const int ADD = 91;
		public const int SUBTRACT = 92;
		public const int BITWISE_OR = 93;
		public const int MULTIPLY = 94;
		public const int DIVISION = 95;
		public const int MODULUS = 96;
		public const int EXPONENTIATION = 97;
		public const int INCREMENT = 98;
		public const int DECREMENT = 99;
		public const int DOT = 100;
		public const int INT = 101;
		public const int DOUBLE_QUOTED_STRING = 102;
		public const int SINGLE_QUOTED_STRING = 103;
		public const int LBRACE = 104;
		public const int RBRACE = 105;
		public const int RE_LITERAL = 106;
		public const int LINE_CONTINUATION = 107;
		public const int SL_COMMENT = 108;
		public const int ML_COMMENT = 109;
		public const int WS = 110;
		public const int NEWLINE = 111;
		public const int ESCAPED_EXPRESSION = 112;
		public const int DQS_ESC = 113;
		public const int SQS_ESC = 114;
		public const int SESC = 115;
		public const int RE_CHAR = 116;
		public const int RE_ESC = 117;
		public const int ID_LETTER = 118;
		public const int DIGIT = 119;
		
				
	protected System.Text.StringBuilder _sbuilder = new System.Text.StringBuilder();
	
	protected AttributeCollection _attributes = new AttributeCollection();
	
	protected TypeMemberModifiers _modifiers = TypeMemberModifiers.None;

	protected bool _inTuple;	
	
	protected void ResetMemberData()
	{
		_modifiers = TypeMemberModifiers.None;
	}

	protected void AddAttributes(AttributeCollection target)
	{
		target.Extend(_attributes);
		_attributes.Clear();
	}

	protected LexicalInfo ToLexicalInfo(antlr.Token token)
	{
		int line = token.getLine();
		int startColumn = token.getColumn();
		int endColumn = token.getColumn() + token.getText().Length;
		string filename = token.getFilename();
		return new LexicalInfo(filename, line, startColumn, endColumn);
	}

	protected BinaryOperatorType ParseCmpOperator(string op)
	{
		switch (op)
		{
			case "<": return BinaryOperatorType.LessThan;
			case "<=": return BinaryOperatorType.LessThanOrEqual;
			case ">": return BinaryOperatorType.GreaterThan;
			case ">=": return BinaryOperatorType.GreaterThanOrEqual;
			case "==": return BinaryOperatorType.Equality;
			case "!=": return BinaryOperatorType.Inequality;
			case "=~": return BinaryOperatorType.Match;
			case "!~": return BinaryOperatorType.NotMatch;
		}
		throw new ArgumentException("op");
	}

	protected BinaryOperatorType ParseAssignOperator(string op)
	{
		switch (op)
		{
			case "=": return BinaryOperatorType.Assign;
			case "+=": return BinaryOperatorType.InPlaceAdd;
			case "-=": return BinaryOperatorType.InPlaceSubtract;
			case "/=": return BinaryOperatorType.InPlaceDivide;
			case "*=": return BinaryOperatorType.InPlaceMultiply;
		}
		throw new ArgumentException(op, "op");
	}

	static double ParseDouble(string text)
	{
		return double.Parse(text, System.Globalization.CultureInfo.InvariantCulture);
	}

	protected TimeSpan ParseTimeSpan(string text)
	{
		if (text.EndsWith("ms"))
		{
			return TimeSpan.FromMilliseconds(ParseDouble(text.Substring(0, text.Length-2)));
		}
	
		char last = text[text.Length-1];		
		double value = ParseDouble(text.Substring(0, text.Length-1));
		switch (last)
		{
			case 's':
			{
				return TimeSpan.FromSeconds(value);
			}
	
			case 'h':
			{
				return TimeSpan.FromHours(value);
			}
			
			case 'm':
			{
				return TimeSpan.FromMinutes(value);
			}			
		}
		return TimeSpan.FromDays(value); 
	}

	// every new line is transformed to '\n'
	// trailing and leading newlines are removed
	protected string MassageDocString(string s)
	{			
		if (s.Length != 0)
		{						
			s = s.Replace("\r\n", "\n");
			
			int length = s.Length;
			int startIndex = 0;			
			if ('\n' == s[0])
			{			
				// assumes '\n'
				startIndex++;
				length--;
			}						
			if ('\n' == s[s.Length-1])
			{
				length--;
			}
			
			if (length > 0)
			{
				return s.Substring(startIndex, length);
			}
			return string.Empty;
		}
		return s;
	}

	protected bool IsValidMacroArgument(int token)
	{
		return LPAREN != token && LBRACK != token;
	}
		
		protected void initialize()
		{
			tokenNames = tokenNames_;
		}
		
		
		protected BooParserBase(TokenBuffer tokenBuf, int k) : base(tokenBuf, k)
		{
			initialize();
		}
		
		public BooParserBase(TokenBuffer tokenBuf) : this(tokenBuf,2)
		{
		}
		
		protected BooParserBase(TokenStream lexer, int k) : base(lexer,k)
		{
			initialize();
		}
		
		public BooParserBase(TokenStream lexer) : this(lexer,2)
		{
		}
		
		public BooParserBase(ParserSharedInputState state) : base(state,2)
		{
			initialize();
		}
		
	protected Module  start() //throws RecognitionException, TokenStreamException
{
		Module module;
		
		
				module = new Module();		
				module.LexicalInfo = new LexicalInfo(getFilename(), 0, 0, 0);
			
		
		try {      // for error handling
			docstring(module);
			{    // ( ... )*
				for (;;)
				{
					if ((LA(1)==EOS) && (tokenSet_0_.member(LA(2))))
					{
						match(EOS);
					}
					else
					{
						goto _loop3_breakloop;
					}
					
				}
_loop3_breakloop:				;
			}    // ( ... )*
			{
				switch ( LA(1) )
				{
				case NAMESPACE:
				{
					namespace_directive(module);
					break;
				}
				case EOF:
				case TIMESPAN:
				case DOUBLE:
				case LONG:
				case ABSTRACT:
				case BREAK:
				case CONTINUE:
				case CAST:
				case CLASS:
				case DEF:
				case ENUM:
				case FINAL:
				case FOR:
				case FALSE:
				case GIVEN:
				case IMPORT:
				case INTERFACE:
				case INTERNAL:
				case IF:
				case NOT:
				case NULL:
				case OVERRIDE:
				case PUBLIC:
				case PROTECTED:
				case PRIVATE:
				case RAISE:
				case RETURN:
				case RETRY:
				case SELF:
				case SUPER:
				case STATIC:
				case TRY:
				case TRANSIENT:
				case TRUE:
				case TYPEOF:
				case UNLESS:
				case VIRTUAL:
				case WHILE:
				case YIELD:
				case EOS:
				case TRIPLE_QUOTED_STRING:
				case ID:
				case LBRACK:
				case LPAREN:
				case SUBTRACT:
				case INCREMENT:
				case DECREMENT:
				case INT:
				case DOUBLE_QUOTED_STRING:
				case SINGLE_QUOTED_STRING:
				case LBRACE:
				case RE_LITERAL:
				{
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			{    // ( ... )*
				for (;;)
				{
					if ((LA(1)==IMPORT))
					{
						import_directive(module);
					}
					else
					{
						goto _loop6_breakloop;
					}
					
				}
_loop6_breakloop:				;
			}    // ( ... )*
			{    // ( ... )*
				for (;;)
				{
					if ((tokenSet_1_.member(LA(1))) && (tokenSet_2_.member(LA(2))))
					{
						type_member(module.Members);
					}
					else
					{
						goto _loop8_breakloop;
					}
					
				}
_loop8_breakloop:				;
			}    // ( ... )*
			globals(module);
			match(Token.EOF_TYPE);
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_3_);
			}
			else
			{
				throw;
			}
		}
		return module;
	}
	
	protected void docstring(
		Node node
	) //throws RecognitionException, TokenStreamException
{
		
		Token  doc = null;
		
		try {      // for error handling
			{
				if ((LA(1)==TRIPLE_QUOTED_STRING) && (tokenSet_4_.member(LA(2))))
				{
					doc = LT(1);
					match(TRIPLE_QUOTED_STRING);
					if (0==inputState.guessing)
					{
						node.Documentation = MassageDocString(doc.getText());
					}
					{    // ( ... )*
						for (;;)
						{
							if ((LA(1)==EOS) && (tokenSet_4_.member(LA(2))))
							{
								match(EOS);
							}
							else
							{
								goto _loop12_breakloop;
							}
							
						}
_loop12_breakloop:						;
					}    // ( ... )*
				}
				else if ((tokenSet_4_.member(LA(1))) && (tokenSet_5_.member(LA(2)))) {
				}
				else
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_4_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected void namespace_directive(
		Module container
	) //throws RecognitionException, TokenStreamException
{
		
		Token  t = null;
		
				Token id;
				NamespaceDeclaration p = null;
			
		
		try {      // for error handling
			t = LT(1);
			match(NAMESPACE);
			id=identifier();
			if (0==inputState.guessing)
			{
				
						p = new NamespaceDeclaration(ToLexicalInfo(t));
						p.Name = id.getText();
						container.Namespace = p; 
					
			}
			eos();
			docstring(p);
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_6_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected void import_directive(
		Module container
	) //throws RecognitionException, TokenStreamException
{
		
		Token  t = null;
		Token  alias = null;
		
				Token id;
				Import usingNode = null;
			
		
		try {      // for error handling
			t = LT(1);
			match(IMPORT);
			id=identifier();
			if (0==inputState.guessing)
			{
				
						usingNode = new Import(ToLexicalInfo(t));
						usingNode.Namespace = id.getText();
						container.Imports.Add(usingNode);
					
			}
			{
				switch ( LA(1) )
				{
				case FROM:
				{
					match(FROM);
					id=identifier();
					if (0==inputState.guessing)
					{
						
									usingNode.AssemblyReference = new ReferenceExpression(ToLexicalInfo(id));
									usingNode.AssemblyReference.Name = id.getText();
								
					}
					break;
				}
				case AS:
				case EOS:
				{
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			{
				switch ( LA(1) )
				{
				case AS:
				{
					match(AS);
					alias = LT(1);
					match(ID);
					if (0==inputState.guessing)
					{
						
									usingNode.Alias = new ReferenceExpression(ToLexicalInfo(alias));
									usingNode.Alias.Name = alias.getText();
								
					}
					break;
				}
				case EOS:
				{
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			eos();
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_6_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected void type_member(
		TypeMemberCollection container
	) //throws RecognitionException, TokenStreamException
{
		
		
		try {      // for error handling
			attributes();
			modifiers();
			{
				switch ( LA(1) )
				{
				case CLASS:
				case ENUM:
				case INTERFACE:
				{
					type_definition(container);
					break;
				}
				case DEF:
				{
					method(container);
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_7_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected void globals(
		Module container
	) //throws RecognitionException, TokenStreamException
{
		
		
		try {      // for error handling
			{    // ( ... )*
				for (;;)
				{
					if ((LA(1)==EOS))
					{
						match(EOS);
					}
					else
					{
						goto _loop101_breakloop;
					}
					
				}
_loop101_breakloop:				;
			}    // ( ... )*
			{    // ( ... )*
				for (;;)
				{
					if ((tokenSet_8_.member(LA(1))))
					{
						stmt(container.Globals.Statements);
					}
					else
					{
						goto _loop103_breakloop;
					}
					
				}
_loop103_breakloop:				;
			}    // ( ... )*
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_3_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected void eos() //throws RecognitionException, TokenStreamException
{
		
		
		try {      // for error handling
			{ // ( ... )+
			int _cnt15=0;
			for (;;)
			{
				if ((LA(1)==EOS) && (tokenSet_9_.member(LA(2))))
				{
					match(EOS);
				}
				else
				{
					if (_cnt15 >= 1) { goto _loop15_breakloop; } else { throw new NoViableAltException(LT(1), getFilename());; }
				}
				
				_cnt15++;
			}
_loop15_breakloop:			;
			}    // ( ... )+
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_9_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected Token  identifier() //throws RecognitionException, TokenStreamException
{
		Token value;
		
		Token  id1 = null;
		Token  id2 = null;
		
				value = null; _sbuilder.Length = 0;
			
		
		try {      // for error handling
			id1 = LT(1);
			match(ID);
			if (0==inputState.guessing)
			{
									
						_sbuilder.Append(id1.getText());
						value = id1;
					
			}
			{    // ( ... )*
				for (;;)
				{
					if ((LA(1)==DOT) && (LA(2)==ID))
					{
						match(DOT);
						id2 = LT(1);
						match(ID);
						if (0==inputState.guessing)
						{
							_sbuilder.Append('.'); _sbuilder.Append(id2.getText());
						}
					}
					else
					{
						goto _loop295_breakloop;
					}
					
				}
_loop295_breakloop:				;
			}    // ( ... )*
			if (0==inputState.guessing)
			{
				value.setText(_sbuilder.ToString());
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_10_);
			}
			else
			{
				throw;
			}
		}
		return value;
	}
	
	protected void attributes() //throws RecognitionException, TokenStreamException
{
		
		
				_attributes.Clear();
			
		
		try {      // for error handling
			{    // ( ... )*
				for (;;)
				{
					if ((LA(1)==LBRACK))
					{
						match(LBRACK);
						{
							switch ( LA(1) )
							{
							case ID:
							{
								attribute();
								{    // ( ... )*
									for (;;)
									{
										if ((LA(1)==COMMA))
										{
											match(COMMA);
											attribute();
										}
										else
										{
											goto _loop34_breakloop;
										}
										
									}
_loop34_breakloop:									;
								}    // ( ... )*
								break;
							}
							case RBRACK:
							{
								break;
							}
							default:
							{
								throw new NoViableAltException(LT(1), getFilename());
							}
							 }
						}
						match(RBRACK);
						{    // ( ... )*
							for (;;)
							{
								if ((LA(1)==EOS))
								{
									match(EOS);
								}
								else
								{
									goto _loop36_breakloop;
								}
								
							}
_loop36_breakloop:							;
						}    // ( ... )*
					}
					else
					{
						goto _loop37_breakloop;
					}
					
				}
_loop37_breakloop:				;
			}    // ( ... )*
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_11_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected void modifiers() //throws RecognitionException, TokenStreamException
{
		
		
				_modifiers = TypeMemberModifiers.None;
			
		
		try {      // for error handling
			{    // ( ... )*
				for (;;)
				{
					switch ( LA(1) )
					{
					case STATIC:
					{
						match(STATIC);
						if (0==inputState.guessing)
						{
							_modifiers |= TypeMemberModifiers.Static;
						}
						break;
					}
					case PUBLIC:
					{
						match(PUBLIC);
						if (0==inputState.guessing)
						{
							_modifiers |= TypeMemberModifiers.Public;
						}
						break;
					}
					case PROTECTED:
					{
						match(PROTECTED);
						if (0==inputState.guessing)
						{
							_modifiers |= TypeMemberModifiers.Protected;
						}
						break;
					}
					case PRIVATE:
					{
						match(PRIVATE);
						if (0==inputState.guessing)
						{
							_modifiers |= TypeMemberModifiers.Private;
						}
						break;
					}
					case INTERNAL:
					{
						match(INTERNAL);
						if (0==inputState.guessing)
						{
							_modifiers |= TypeMemberModifiers.Internal;
						}
						break;
					}
					case FINAL:
					{
						match(FINAL);
						if (0==inputState.guessing)
						{
							_modifiers |= TypeMemberModifiers.Final;
						}
						break;
					}
					case TRANSIENT:
					{
						match(TRANSIENT);
						if (0==inputState.guessing)
						{
							_modifiers |= TypeMemberModifiers.Transient;
						}
						break;
					}
					case OVERRIDE:
					{
						match(OVERRIDE);
						if (0==inputState.guessing)
						{
							_modifiers |= TypeMemberModifiers.Override;
						}
						break;
					}
					case ABSTRACT:
					{
						match(ABSTRACT);
						if (0==inputState.guessing)
						{
							_modifiers |= TypeMemberModifiers.Abstract;
						}
						break;
					}
					case VIRTUAL:
					{
						match(VIRTUAL);
						if (0==inputState.guessing)
						{
							_modifiers |= TypeMemberModifiers.Virtual;
						}
						break;
					}
					default:
					{
						goto _loop113_breakloop;
					}
					 }
				}
_loop113_breakloop:				;
			}    // ( ... )*
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_12_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected void type_definition(
		TypeMemberCollection container
	) //throws RecognitionException, TokenStreamException
{
		
		
		try {      // for error handling
			{
				switch ( LA(1) )
				{
				case CLASS:
				{
					class_definition(container);
					break;
				}
				case INTERFACE:
				{
					interface_definition(container);
					break;
				}
				case ENUM:
				{
					enum_definition(container);
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_13_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected void method(
		TypeMemberCollection container
	) //throws RecognitionException, TokenStreamException
{
		
		Token  t = null;
		Token  id = null;
		Token  c = null;
		
				Method m = null;
				TypeReference rt = null;
			
		
		try {      // for error handling
			t = LT(1);
			match(DEF);
			{
				switch ( LA(1) )
				{
				case ID:
				{
					id = LT(1);
					match(ID);
					if (0==inputState.guessing)
					{
						m = new Method(ToLexicalInfo(t)); m.Name = id.getText();
					}
					break;
				}
				case CONSTRUCTOR:
				{
					c = LT(1);
					match(CONSTRUCTOR);
					if (0==inputState.guessing)
					{
						m = new Constructor(ToLexicalInfo(t));
					}
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			if (0==inputState.guessing)
			{
				
						m.Modifiers = _modifiers;
						AddAttributes(m.Attributes);
					
			}
			match(LPAREN);
			parameter_declaration_list(m.Parameters);
			match(RPAREN);
			{
				switch ( LA(1) )
				{
				case AS:
				{
					match(AS);
					rt=type_reference();
					if (0==inputState.guessing)
					{
						m.ReturnType = rt;
					}
					break;
				}
				case LBRACK:
				case COLON:
				{
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			attributes();
			if (0==inputState.guessing)
			{
				AddAttributes(m.ReturnTypeAttributes);
			}
			begin_with_doc(m);
			block(m.Body.Statements);
			end();
			if (0==inputState.guessing)
			{
				container.Add(m);
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_13_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected void class_definition(
		TypeMemberCollection container
	) //throws RecognitionException, TokenStreamException
{
		
		Token  c = null;
		Token  id = null;
		
				ClassDefinition cd = null;
			
		
		try {      // for error handling
			c = LT(1);
			match(CLASS);
			id = LT(1);
			match(ID);
			if (0==inputState.guessing)
			{
				
						cd = new ClassDefinition(ToLexicalInfo(c));
						cd.Name = id.getText();
						cd.Modifiers = _modifiers;
						AddAttributes(cd.Attributes);
						container.Add(cd);
					
			}
			{
				switch ( LA(1) )
				{
				case LPAREN:
				{
					base_types(cd.BaseTypes);
					break;
				}
				case COLON:
				{
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			begin_with_doc(cd);
			{
				switch ( LA(1) )
				{
				case PASS:
				{
					{
						match(PASS);
						eos();
					}
					break;
				}
				case ABSTRACT:
				case CLASS:
				case DEF:
				case ENUM:
				case FINAL:
				case INTERFACE:
				case INTERNAL:
				case OVERRIDE:
				case PUBLIC:
				case PROTECTED:
				case PRIVATE:
				case STATIC:
				case TRANSIENT:
				case VIRTUAL:
				case EOS:
				case ID:
				case LBRACK:
				{
					{ // ( ... )+
					int _cnt48=0;
					for (;;)
					{
						if ((tokenSet_14_.member(LA(1))))
						{
							{    // ( ... )*
								for (;;)
								{
									if ((LA(1)==EOS))
									{
										match(EOS);
									}
									else
									{
										goto _loop46_breakloop;
									}
									
								}
_loop46_breakloop:								;
							}    // ( ... )*
							attributes();
							modifiers();
							{
								switch ( LA(1) )
								{
								case DEF:
								{
									method(cd.Members);
									break;
								}
								case ID:
								{
									field_or_property(cd.Members);
									break;
								}
								case CLASS:
								case ENUM:
								case INTERFACE:
								{
									type_definition(cd.Members);
									break;
								}
								default:
								{
									throw new NoViableAltException(LT(1), getFilename());
								}
								 }
							}
						}
						else
						{
							if (_cnt48 >= 1) { goto _loop48_breakloop; } else { throw new NoViableAltException(LT(1), getFilename());; }
						}
						
						_cnt48++;
					}
_loop48_breakloop:					;
					}    // ( ... )+
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			end();
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_13_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected void interface_definition(
		TypeMemberCollection container
	) //throws RecognitionException, TokenStreamException
{
		
		Token  it = null;
		Token  id = null;
		
				InterfaceDefinition itf = null;
			
		
		try {      // for error handling
			it = LT(1);
			match(INTERFACE);
			id = LT(1);
			match(ID);
			if (0==inputState.guessing)
			{
				
						itf = new InterfaceDefinition(ToLexicalInfo(it));
						itf.Name = id.getText();
						itf.Modifiers = _modifiers;
						AddAttributes(itf.Attributes);
						container.Add(itf);
					
			}
			{
				switch ( LA(1) )
				{
				case LPAREN:
				{
					base_types(itf.BaseTypes);
					break;
				}
				case COLON:
				{
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			begin();
			{
				switch ( LA(1) )
				{
				case PASS:
				{
					{
						match(PASS);
						eos();
					}
					break;
				}
				case DEF:
				case ID:
				case LBRACK:
				{
					{ // ( ... )+
					int _cnt55=0;
					for (;;)
					{
						if ((LA(1)==DEF||LA(1)==ID||LA(1)==LBRACK))
						{
							attributes();
							{
								switch ( LA(1) )
								{
								case DEF:
								{
									interface_method(itf.Members);
									break;
								}
								case ID:
								{
									interface_property(itf.Members);
									break;
								}
								default:
								{
									throw new NoViableAltException(LT(1), getFilename());
								}
								 }
							}
						}
						else
						{
							if (_cnt55 >= 1) { goto _loop55_breakloop; } else { throw new NoViableAltException(LT(1), getFilename());; }
						}
						
						_cnt55++;
					}
_loop55_breakloop:					;
					}    // ( ... )+
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			end();
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_13_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected void enum_definition(
		TypeMemberCollection container
	) //throws RecognitionException, TokenStreamException
{
		
		Token  id = null;
		
				EnumDefinition ed = null;
			
		
		try {      // for error handling
			match(ENUM);
			id = LT(1);
			match(ID);
			begin();
			if (0==inputState.guessing)
			{
				
						ed = new EnumDefinition(ToLexicalInfo(id));
						ed.Name = id.getText();
						ed.Modifiers = _modifiers;
						AddAttributes(ed.Attributes);
						container.Add(ed);
					
			}
			{
				{ // ( ... )+
				int _cnt27=0;
				for (;;)
				{
					if ((LA(1)==ID||LA(1)==LBRACK))
					{
						enum_member(ed);
					}
					else
					{
						if (_cnt27 >= 1) { goto _loop27_breakloop; } else { throw new NoViableAltException(LT(1), getFilename());; }
					}
					
					_cnt27++;
				}
_loop27_breakloop:				;
				}    // ( ... )+
			}
			end();
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_13_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected void begin() //throws RecognitionException, TokenStreamException
{
		
		
		try {      // for error handling
			match(COLON);
			match(INDENT);
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_15_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected void enum_member(
		EnumDefinition container
	) //throws RecognitionException, TokenStreamException
{
		
		Token  id = null;
				
				IntegerLiteralExpression initializer = null;		
			
		
		try {      // for error handling
			attributes();
			id = LT(1);
			match(ID);
			{
				switch ( LA(1) )
				{
				case ASSIGN:
				{
					match(ASSIGN);
					initializer=integer_literal();
					break;
				}
				case EOS:
				{
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			if (0==inputState.guessing)
			{
				
						EnumMember em = new EnumMember(ToLexicalInfo(id));
						em.Name = id.getText();
						em.Initializer = initializer;
						AddAttributes(em.Attributes);
						container.Members.Add(em);
					
			}
			eos();
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_16_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected void end() //throws RecognitionException, TokenStreamException
{
		
		
		try {      // for error handling
			match(DEDENT);
			{    // ( ... )*
				for (;;)
				{
					if ((LA(1)==EOS) && (tokenSet_17_.member(LA(2))))
					{
						match(EOS);
					}
					else
					{
						goto _loop128_breakloop;
					}
					
				}
_loop128_breakloop:				;
			}    // ( ... )*
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_17_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected IntegerLiteralExpression  integer_literal() //throws RecognitionException, TokenStreamException
{
		IntegerLiteralExpression e;
		
		Token  i = null;
		Token  l = null;
		e = null;
		
		try {      // for error handling
			switch ( LA(1) )
			{
			case INT:
			{
				i = LT(1);
				match(INT);
				if (0==inputState.guessing)
				{
					
							e = new IntegerLiteralExpression(ToLexicalInfo(i), long.Parse(i.getText()));
						
				}
				break;
			}
			case LONG:
			{
				l = LT(1);
				match(LONG);
				if (0==inputState.guessing)
				{
					
							string value = l.getText();
							value = value.Substring(0, value.Length-1);
							
							e = new IntegerLiteralExpression(ToLexicalInfo(l),
										long.Parse(value),
										true);
						
				}
				break;
			}
			default:
			{
				throw new NoViableAltException(LT(1), getFilename());
			}
			 }
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return e;
	}
	
	protected void attribute() //throws RecognitionException, TokenStreamException
{
		
				
				antlr.Token id = null;
				Boo.Lang.Ast.Attribute attr = null;
			
		
		try {      // for error handling
			id=identifier();
			if (0==inputState.guessing)
			{
				
						attr = new Boo.Lang.Ast.Attribute(ToLexicalInfo(id));
						attr.Name = id.getText();
						_attributes.Add(attr);
					
			}
			{
				switch ( LA(1) )
				{
				case LPAREN:
				{
					match(LPAREN);
					parameter_list(attr);
					match(RPAREN);
					break;
				}
				case COMMA:
				case RBRACK:
				{
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_19_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected void parameter_list(
		INodeWithArguments node
	) //throws RecognitionException, TokenStreamException
{
		
		
		try {      // for error handling
			{
				switch ( LA(1) )
				{
				case TIMESPAN:
				case DOUBLE:
				case LONG:
				case CAST:
				case FALSE:
				case NOT:
				case NULL:
				case SELF:
				case SUPER:
				case TRUE:
				case TYPEOF:
				case TRIPLE_QUOTED_STRING:
				case ID:
				case LBRACK:
				case LPAREN:
				case SUBTRACT:
				case INCREMENT:
				case DECREMENT:
				case INT:
				case DOUBLE_QUOTED_STRING:
				case SINGLE_QUOTED_STRING:
				case LBRACE:
				case RE_LITERAL:
				{
					parameter(node);
					{    // ( ... )*
						for (;;)
						{
							if ((LA(1)==COMMA))
							{
								match(COMMA);
								parameter(node);
							}
							else
							{
								goto _loop289_breakloop;
							}
							
						}
_loop289_breakloop:						;
					}    // ( ... )*
					break;
				}
				case RPAREN:
				{
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_20_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected void base_types(
		TypeReferenceCollection container
	) //throws RecognitionException, TokenStreamException
{
		
		
				TypeReference tr = null;
			
		
		try {      // for error handling
			match(LPAREN);
			tr=type_reference();
			if (0==inputState.guessing)
			{
				container.Add(tr);
			}
			{    // ( ... )*
				for (;;)
				{
					if ((LA(1)==COMMA))
					{
						match(COMMA);
						tr=type_reference();
						if (0==inputState.guessing)
						{
							container.Add(tr);
						}
					}
					else
					{
						goto _loop58_breakloop;
					}
					
				}
_loop58_breakloop:				;
			}    // ( ... )*
			match(RPAREN);
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_21_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected void begin_with_doc(
		Node node
	) //throws RecognitionException, TokenStreamException
{
		
		
		try {      // for error handling
			match(COLON);
			{
				switch ( LA(1) )
				{
				case EOS:
				{
					match(EOS);
					docstring(node);
					break;
				}
				case INDENT:
				{
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			match(INDENT);
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_22_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected void field_or_property(
		TypeMemberCollection container
	) //throws RecognitionException, TokenStreamException
{
		
		Token  id = null;
		
				TypeMember tm = null;
				TypeReference tr = null;
				Property p = null;
				Expression initializer = null;
			
		
		try {      // for error handling
			id = LT(1);
			match(ID);
			{
				bool synPredMatched86 = false;
				if (((LA(1)==AS||LA(1)==LPAREN||LA(1)==COLON) && (tokenSet_23_.member(LA(2)))))
				{
					int _m86 = mark();
					synPredMatched86 = true;
					inputState.guessing++;
					try {
						{
							property_header();
						}
					}
					catch (RecognitionException)
					{
						synPredMatched86 = false;
					}
					rewind(_m86);
					inputState.guessing--;
				}
				if ( synPredMatched86 )
				{
					{
						if (0==inputState.guessing)
						{
							p = new Property(ToLexicalInfo(id));
						}
						{
							switch ( LA(1) )
							{
							case LPAREN:
							{
								match(LPAREN);
								parameter_declaration_list(p.Parameters);
								match(RPAREN);
								break;
							}
							case AS:
							case COLON:
							{
								break;
							}
							default:
							{
								throw new NoViableAltException(LT(1), getFilename());
							}
							 }
						}
						{
							switch ( LA(1) )
							{
							case AS:
							{
								match(AS);
								tr=type_reference();
								break;
							}
							case COLON:
							{
								break;
							}
							default:
							{
								throw new NoViableAltException(LT(1), getFilename());
							}
							 }
						}
						if (0==inputState.guessing)
						{
														
											p.Type = tr;
											tm = p;
											tm.Name = id.getText();
											tm.Modifiers = _modifiers;
											AddAttributes(tm.Attributes);
										
						}
						begin_with_doc(p);
						{ // ( ... )+
						int _cnt91=0;
						for (;;)
						{
							if ((tokenSet_24_.member(LA(1))))
							{
								property_accessor(p);
							}
							else
							{
								if (_cnt91 >= 1) { goto _loop91_breakloop; } else { throw new NoViableAltException(LT(1), getFilename());; }
							}
							
							_cnt91++;
						}
_loop91_breakloop:						;
						}    // ( ... )+
						end();
					}
				}
				else if ((LA(1)==AS||LA(1)==EOS||LA(1)==ASSIGN) && (tokenSet_25_.member(LA(2)))) {
					{
						{
							switch ( LA(1) )
							{
							case AS:
							{
								match(AS);
								tr=type_reference();
								break;
							}
							case EOS:
							case ASSIGN:
							{
								break;
							}
							default:
							{
								throw new NoViableAltException(LT(1), getFilename());
							}
							 }
						}
						{
							switch ( LA(1) )
							{
							case ASSIGN:
							{
								match(ASSIGN);
								initializer=expression();
								break;
							}
							case EOS:
							{
								break;
							}
							default:
							{
								throw new NoViableAltException(LT(1), getFilename());
							}
							 }
						}
						eos();
						if (0==inputState.guessing)
						{
							
											Field field = new Field(ToLexicalInfo(id));
											field.Type = tr;
											field.Initializer = initializer;
											tm = field;
											tm.Name = id.getText();
											tm.Modifiers = _modifiers;
											AddAttributes(tm.Attributes);
										
						}
						docstring(tm);
					}
				}
				else
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				
			}
			if (0==inputState.guessing)
			{
				container.Add(tm);
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_26_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected void interface_method(
		TypeMemberCollection container
	) //throws RecognitionException, TokenStreamException
{
		
		Token  t = null;
		Token  id = null;
		
				Method m = null;
				TypeReference rt = null;
			
		
		try {      // for error handling
			t = LT(1);
			match(DEF);
			id = LT(1);
			match(ID);
			if (0==inputState.guessing)
			{
				
						m = new Method(ToLexicalInfo(t));
						m.Name = id.getText();
						AddAttributes(m.Attributes);
						container.Add(m);
					
			}
			match(LPAREN);
			parameter_declaration_list(m.Parameters);
			match(RPAREN);
			{
				switch ( LA(1) )
				{
				case AS:
				{
					match(AS);
					rt=type_reference();
					if (0==inputState.guessing)
					{
						m.ReturnType=rt;
					}
					break;
				}
				case EOS:
				case COLON:
				{
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			{
				switch ( LA(1) )
				{
				case EOS:
				{
					eos();
					break;
				}
				case COLON:
				{
					{
						empty_block();
						{    // ( ... )*
							for (;;)
							{
								if ((LA(1)==EOS))
								{
									match(EOS);
								}
								else
								{
									goto _loop64_breakloop;
								}
								
							}
_loop64_breakloop:							;
						}    // ( ... )*
					}
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_27_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected void interface_property(
		TypeMemberCollection container
	) //throws RecognitionException, TokenStreamException
{
		
		Token  id = null;
		
				Property p = null;
				TypeReference tr = null;
			
		
		try {      // for error handling
			id = LT(1);
			match(ID);
			{
				switch ( LA(1) )
				{
				case AS:
				{
					match(AS);
					tr=type_reference();
					break;
				}
				case COLON:
				{
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			if (0==inputState.guessing)
			{
				
						p = new Property(ToLexicalInfo(id));
						p.Name = id.getText();
						p.Type = tr;
						AddAttributes(p.Attributes);
						container.Add(p);
					
			}
			begin();
			{ // ( ... )+
			int _cnt68=0;
			for (;;)
			{
				if ((LA(1)==GET||LA(1)==SET||LA(1)==LBRACK))
				{
					interface_property_accessor(p);
				}
				else
				{
					if (_cnt68 >= 1) { goto _loop68_breakloop; } else { throw new NoViableAltException(LT(1), getFilename());; }
				}
				
				_cnt68++;
			}
_loop68_breakloop:			;
			}    // ( ... )+
			end();
			{    // ( ... )*
				for (;;)
				{
					if ((LA(1)==EOS))
					{
						match(EOS);
					}
					else
					{
						goto _loop70_breakloop;
					}
					
				}
_loop70_breakloop:				;
			}    // ( ... )*
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_27_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected TypeReference  type_reference() //throws RecognitionException, TokenStreamException
{
		TypeReference tr;
		
		Token  lparen = null;
		Token  rparen = null;
		
				tr=null;
				Token id = null;
			
		
		try {      // for error handling
			switch ( LA(1) )
			{
			case LPAREN:
			{
				{
					lparen = LT(1);
					match(LPAREN);
					tr=type_reference();
					rparen = LT(1);
					match(RPAREN);
					if (0==inputState.guessing)
					{
						
									TupleTypeReference ttr = new TupleTypeReference(ToLexicalInfo(lparen));
									ttr.ElementType = tr;
									tr = ttr;
								
					}
				}
				break;
			}
			case ID:
			{
				{
					id=identifier();
					if (0==inputState.guessing)
					{
						
									SimpleTypeReference str = new SimpleTypeReference(ToLexicalInfo(id));
									str.Name = id.getText();
									tr = str;
								
					}
				}
				break;
			}
			default:
			{
				throw new NoViableAltException(LT(1), getFilename());
			}
			 }
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return tr;
	}
	
	protected void parameter_declaration_list(
		ParameterDeclarationCollection c
	) //throws RecognitionException, TokenStreamException
{
		
		
		try {      // for error handling
			{
				switch ( LA(1) )
				{
				case ID:
				case LBRACK:
				{
					parameter_declaration(c);
					{    // ( ... )*
						for (;;)
						{
							if ((LA(1)==COMMA))
							{
								match(COMMA);
								parameter_declaration(c);
							}
							else
							{
								goto _loop117_breakloop;
							}
							
						}
_loop117_breakloop:						;
					}    // ( ... )*
					break;
				}
				case RPAREN:
				{
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_20_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected void empty_block() //throws RecognitionException, TokenStreamException
{
		
		
		try {      // for error handling
			begin();
			match(PASS);
			eos();
			end();
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_28_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected void interface_property_accessor(
		Property p
	) //throws RecognitionException, TokenStreamException
{
		
		Token  gt = null;
		Token  st = null;
		
				Method m = null;
			
		
		try {      // for error handling
			attributes();
			{
				if (((LA(1)==GET))&&( null == p.Getter ))
				{
					{
						gt = LT(1);
						match(GET);
						if (0==inputState.guessing)
						{
							m = p.Getter = new Method(ToLexicalInfo(gt)); m.Name = "get";
						}
					}
				}
				else if (((LA(1)==SET))&&( null == p.Setter )) {
					{
						st = LT(1);
						match(SET);
						if (0==inputState.guessing)
						{
							m = p.Setter = new Method(ToLexicalInfo(st)); m.Name = "set";
						}
					}
				}
				else
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				
			}
			{
				switch ( LA(1) )
				{
				case EOS:
				{
					eos();
					break;
				}
				case COLON:
				{
					empty_block();
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			if (0==inputState.guessing)
			{
				
						AddAttributes(m.Attributes);
					
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_29_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected void block(
		StatementCollection container
	) //throws RecognitionException, TokenStreamException
{
		
		
		try {      // for error handling
			{    // ( ... )*
				for (;;)
				{
					if ((LA(1)==EOS))
					{
						match(EOS);
					}
					else
					{
						goto _loop106_breakloop;
					}
					
				}
_loop106_breakloop:				;
			}    // ( ... )*
			{
				switch ( LA(1) )
				{
				case PASS:
				{
					{
						match(PASS);
						eos();
					}
					break;
				}
				case TIMESPAN:
				case DOUBLE:
				case LONG:
				case BREAK:
				case CONTINUE:
				case CAST:
				case FOR:
				case FALSE:
				case GIVEN:
				case IF:
				case NOT:
				case NULL:
				case RAISE:
				case RETURN:
				case RETRY:
				case SELF:
				case SUPER:
				case TRY:
				case TRUE:
				case TYPEOF:
				case UNLESS:
				case WHILE:
				case YIELD:
				case TRIPLE_QUOTED_STRING:
				case ID:
				case LBRACK:
				case LPAREN:
				case SUBTRACT:
				case INCREMENT:
				case DECREMENT:
				case INT:
				case DOUBLE_QUOTED_STRING:
				case SINGLE_QUOTED_STRING:
				case LBRACE:
				case RE_LITERAL:
				{
					{ // ( ... )+
					int _cnt110=0;
					for (;;)
					{
						if ((tokenSet_8_.member(LA(1))))
						{
							stmt(container);
						}
						else
						{
							if (_cnt110 >= 1) { goto _loop110_breakloop; } else { throw new NoViableAltException(LT(1), getFilename());; }
						}
						
						_cnt110++;
					}
_loop110_breakloop:					;
					}    // ( ... )+
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_30_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected void property_header() //throws RecognitionException, TokenStreamException
{
		
		
		try {      // for error handling
			switch ( LA(1) )
			{
			case LPAREN:
			{
				match(LPAREN);
				break;
			}
			case AS:
			case COLON:
			{
				{
					{
						switch ( LA(1) )
						{
						case AS:
						{
							match(AS);
							type_reference();
							break;
						}
						case COLON:
						{
							break;
						}
						default:
						{
							throw new NoViableAltException(LT(1), getFilename());
						}
						 }
					}
					match(COLON);
				}
				break;
			}
			default:
			{
				throw new NoViableAltException(LT(1), getFilename());
			}
			 }
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_3_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected void property_accessor(
		Property p
	) //throws RecognitionException, TokenStreamException
{
		
		Token  gt = null;
		Token  st = null;
				
				Method m = null;
			
		
		try {      // for error handling
			attributes();
			modifiers();
			{
				if (((LA(1)==GET))&&( null == p.Getter ))
				{
					{
						gt = LT(1);
						match(GET);
						if (0==inputState.guessing)
						{
							
											p.Getter = m = new Method(ToLexicalInfo(gt));		
											m.Name = "get";
										
						}
					}
				}
				else if (((LA(1)==SET))&&( null == p.Setter )) {
					{
						st = LT(1);
						match(SET);
						if (0==inputState.guessing)
						{
							
											p.Setter = m = new Method(ToLexicalInfo(st));
											m.Name = "set";
										
						}
					}
				}
				else
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				
			}
			if (0==inputState.guessing)
			{
				
						AddAttributes(m.Attributes);
						m.Modifiers = _modifiers;
					
			}
			compound_stmt(m.Body.Statements);
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_31_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected Expression  expression() //throws RecognitionException, TokenStreamException
{
		Expression e;
		
		Token  t = null;
		Token  f = null;
		
				e = null;
				TypeReference tr = null;
				
				IteratorExpression lde = null;
				StatementModifier filter = null;
				Expression iterator = null;
				DeclarationCollection declarations = null;
			
		
		try {      // for error handling
			e=boolean_expression();
			{
				if ((LA(1)==AS) && (LA(2)==ID||LA(2)==LPAREN))
				{
					t = LT(1);
					match(AS);
					tr=type_reference();
					if (0==inputState.guessing)
					{
						
									AsExpression ae = new AsExpression(ToLexicalInfo(t));
									ae.Target = e;
									ae.Type = tr;
									e = ae; 
								
					}
				}
				else if ((tokenSet_18_.member(LA(1))) && (tokenSet_32_.member(LA(2)))) {
				}
				else
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				
			}
			{
				if ((LA(1)==FOR) && (LA(2)==ID))
				{
					f = LT(1);
					match(FOR);
					if (0==inputState.guessing)
					{
						
									lde = new IteratorExpression(ToLexicalInfo(f));
									lde.Expression = e;
									
									declarations = lde.Declarations;
								
					}
					declaration_list(declarations);
					match(IN);
					iterator=expression();
					if (0==inputState.guessing)
					{
						lde.Iterator = iterator;
					}
					{
						if ((LA(1)==IF||LA(1)==UNLESS||LA(1)==WHILE) && (tokenSet_33_.member(LA(2))))
						{
							filter=stmt_modifier();
							if (0==inputState.guessing)
							{
								lde.Filter = filter;
							}
						}
						else if ((tokenSet_18_.member(LA(1))) && (tokenSet_32_.member(LA(2)))) {
						}
						else
						{
							throw new NoViableAltException(LT(1), getFilename());
						}
						
					}
					if (0==inputState.guessing)
					{
						e = lde;
					}
				}
				else if ((tokenSet_18_.member(LA(1))) && (tokenSet_32_.member(LA(2)))) {
				}
				else
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return e;
	}
	
	protected void compound_stmt(
		StatementCollection c
	) //throws RecognitionException, TokenStreamException
{
		
		
		try {      // for error handling
			begin();
			block(c);
			end();
			{    // ( ... )*
				for (;;)
				{
					if ((LA(1)==EOS))
					{
						match(EOS);
					}
					else
					{
						goto _loop131_breakloop;
					}
					
				}
_loop131_breakloop:				;
			}    // ( ... )*
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_34_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected void stmt(
		StatementCollection container
	) //throws RecognitionException, TokenStreamException
{
		
		
				Statement s = null;
				StatementModifier m = null;
			
		
		try {      // for error handling
			{
				switch ( LA(1) )
				{
				case FOR:
				{
					s=for_stmt();
					break;
				}
				case WHILE:
				{
					s=while_stmt();
					break;
				}
				case IF:
				{
					s=if_stmt();
					break;
				}
				case UNLESS:
				{
					s=unless_stmt();
					break;
				}
				case TRY:
				{
					s=try_stmt();
					break;
				}
				case GIVEN:
				{
					s=given_stmt();
					break;
				}
				default:
					if (((LA(1)==ID) && (tokenSet_35_.member(LA(2))))&&(IsValidMacroArgument(LA(2))))
					{
						s=macro_stmt();
					}
					else if ((tokenSet_36_.member(LA(1))) && (tokenSet_37_.member(LA(2)))) {
						{
							{
								switch ( LA(1) )
								{
								case RETURN:
								{
									s=return_stmt();
									break;
								}
								case YIELD:
								{
									s=yield_stmt();
									break;
								}
								case BREAK:
								{
									s=break_stmt();
									break;
								}
								case CONTINUE:
								{
									s=continue_stmt();
									break;
								}
								case RAISE:
								{
									s=raise_stmt();
									break;
								}
								case RETRY:
								{
									s=retry_stmt();
									break;
								}
								default:
									bool synPredMatched139 = false;
									if (((LA(1)==ID) && (LA(2)==AS||LA(2)==ASSIGN||LA(2)==COMMA)))
									{
										int _m139 = mark();
										synPredMatched139 = true;
										inputState.guessing++;
										try {
											{
												declaration();
												match(COMMA);
											}
										}
										catch (RecognitionException)
										{
											synPredMatched139 = false;
										}
										rewind(_m139);
										inputState.guessing--;
									}
									if ( synPredMatched139 )
									{
										s=unpack_stmt();
									}
									else if ((LA(1)==ID) && (LA(2)==AS)) {
										s=declaration_stmt();
									}
									else {
										bool synPredMatched141 = false;
										if (((tokenSet_38_.member(LA(1))) && (tokenSet_39_.member(LA(2)))))
										{
											int _m141 = mark();
											synPredMatched141 = true;
											inputState.guessing++;
											try {
												{
													slicing_expression();
													match(ASSIGN);
												}
											}
											catch (RecognitionException)
											{
												synPredMatched141 = false;
											}
											rewind(_m141);
											inputState.guessing--;
										}
										if ( synPredMatched141 )
										{
											s=assignment_stmt();
										}
										else if ((tokenSet_33_.member(LA(1))) && (tokenSet_37_.member(LA(2)))) {
											s=expression_stmt();
										}
									else
									{
										throw new NoViableAltException(LT(1), getFilename());
									}
									}break; }
								}
								{
									switch ( LA(1) )
									{
									case IF:
									case UNLESS:
									case WHILE:
									{
										m=stmt_modifier();
										if (0==inputState.guessing)
										{
											s.Modifier = m;
										}
										break;
									}
									case EOS:
									{
										break;
									}
									default:
									{
										throw new NoViableAltException(LT(1), getFilename());
									}
									 }
								}
								eos();
							}
						}
					else
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					break; }
				}
				if (0==inputState.guessing)
				{
					
							if (null != s)
							{
								container.Add(s);
							}
						
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					consume();
					consumeUntil(tokenSet_40_);
				}
				else
				{
					throw;
				}
			}
		}
		
	protected void parameter_declaration(
		ParameterDeclarationCollection c
	) //throws RecognitionException, TokenStreamException
{
		
		Token  id = null;
				
				TypeReference tr = null;
			
		
		try {      // for error handling
			attributes();
			id = LT(1);
			match(ID);
			{
				switch ( LA(1) )
				{
				case AS:
				{
					match(AS);
					tr=type_reference();
					break;
				}
				case COMMA:
				case RPAREN:
				{
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			if (0==inputState.guessing)
			{
				
						ParameterDeclaration pd = new ParameterDeclaration(ToLexicalInfo(id));
						pd.Name = id.getText();
						pd.Type = tr;
						AddAttributes(pd.Attributes);
						c.Add(pd);
					
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_41_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected MacroStatement  macro_stmt() //throws RecognitionException, TokenStreamException
{
		MacroStatement returnValue;
		
		Token  id = null;
		
				returnValue = null;
				MacroStatement macro = new MacroStatement();
			
		
		try {      // for error handling
			id = LT(1);
			match(ID);
			expression_list(macro.Arguments);
			{
				switch ( LA(1) )
				{
				case COLON:
				{
					compound_stmt(macro.Block.Statements);
					break;
				}
				case EOS:
				{
					eos();
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			if (0==inputState.guessing)
			{
				
						macro.Name = id.getText();
						macro.LexicalInfo = ToLexicalInfo(id);
						
						returnValue = macro;
					
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_40_);
			}
			else
			{
				throw;
			}
		}
		return returnValue;
	}
	
	protected void expression_list(
		ExpressionCollection ec
	) //throws RecognitionException, TokenStreamException
{
		
		
				Expression e = null;
			
		
		try {      // for error handling
			{
				switch ( LA(1) )
				{
				case TIMESPAN:
				case DOUBLE:
				case LONG:
				case CAST:
				case FALSE:
				case NOT:
				case NULL:
				case SELF:
				case SUPER:
				case TRUE:
				case TYPEOF:
				case TRIPLE_QUOTED_STRING:
				case ID:
				case LBRACK:
				case LPAREN:
				case SUBTRACT:
				case INCREMENT:
				case DECREMENT:
				case INT:
				case DOUBLE_QUOTED_STRING:
				case SINGLE_QUOTED_STRING:
				case LBRACE:
				case RE_LITERAL:
				{
					e=expression();
					if (0==inputState.guessing)
					{
						ec.Add(e);
					}
					{    // ( ... )*
						for (;;)
						{
							if ((LA(1)==COMMA))
							{
								match(COMMA);
								e=expression();
								if (0==inputState.guessing)
								{
									ec.Add(e);
								}
							}
							else
							{
								goto _loop285_breakloop;
							}
							
						}
_loop285_breakloop:						;
					}    // ( ... )*
					break;
				}
				case EOS:
				case COLON:
				{
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_42_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected ForStatement  for_stmt() //throws RecognitionException, TokenStreamException
{
		ForStatement fs;
		
		Token  f = null;
		
				fs = null;
				Expression iterator = null;
			
		
		try {      // for error handling
			f = LT(1);
			match(FOR);
			if (0==inputState.guessing)
			{
				fs = new ForStatement(ToLexicalInfo(f));
			}
			declaration_list(fs.Declarations);
			match(IN);
			iterator=tuple_or_expression();
			if (0==inputState.guessing)
			{
				fs.Iterator = iterator;
			}
			compound_stmt(fs.Block.Statements);
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_40_);
			}
			else
			{
				throw;
			}
		}
		return fs;
	}
	
	protected WhileStatement  while_stmt() //throws RecognitionException, TokenStreamException
{
		WhileStatement ws;
		
		Token  w = null;
		
				ws = null;
				Expression e = null;
			
		
		try {      // for error handling
			w = LT(1);
			match(WHILE);
			e=expression();
			if (0==inputState.guessing)
			{
				
						ws = new WhileStatement(ToLexicalInfo(w));
						ws.Condition = e;
					
			}
			compound_stmt(ws.Block.Statements);
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_40_);
			}
			else
			{
				throw;
			}
		}
		return ws;
	}
	
	protected IfStatement  if_stmt() //throws RecognitionException, TokenStreamException
{
		IfStatement s;
		
		Token  it = null;
		Token  et = null;
		
				s = null;
				Expression e = null;
			
		
		try {      // for error handling
			it = LT(1);
			match(IF);
			e=expression();
			if (0==inputState.guessing)
			{
				
						s = new IfStatement(ToLexicalInfo(it));
						s.Condition = e;
						s.TrueBlock = new Block();
					
			}
			compound_stmt(s.TrueBlock.Statements);
			{
				switch ( LA(1) )
				{
				case ELSE:
				{
					et = LT(1);
					match(ELSE);
					if (0==inputState.guessing)
					{
						s.FalseBlock = new Block(ToLexicalInfo(et));
					}
					compound_stmt(s.FalseBlock.Statements);
					break;
				}
				case EOF:
				case TIMESPAN:
				case DOUBLE:
				case LONG:
				case DEDENT:
				case BREAK:
				case CONTINUE:
				case CAST:
				case FOR:
				case FALSE:
				case GIVEN:
				case IF:
				case NOT:
				case NULL:
				case RAISE:
				case RETURN:
				case RETRY:
				case SELF:
				case SUPER:
				case TRY:
				case TRUE:
				case TYPEOF:
				case UNLESS:
				case WHILE:
				case YIELD:
				case TRIPLE_QUOTED_STRING:
				case ID:
				case LBRACK:
				case LPAREN:
				case SUBTRACT:
				case INCREMENT:
				case DECREMENT:
				case INT:
				case DOUBLE_QUOTED_STRING:
				case SINGLE_QUOTED_STRING:
				case LBRACE:
				case RE_LITERAL:
				{
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_40_);
			}
			else
			{
				throw;
			}
		}
		return s;
	}
	
	protected UnlessStatement  unless_stmt() //throws RecognitionException, TokenStreamException
{
		UnlessStatement us;
		
		Token  u = null;
		
				us = null;
				Expression condition = null;
			
		
		try {      // for error handling
			u = LT(1);
			match(UNLESS);
			condition=expression();
			if (0==inputState.guessing)
			{
				
						us = new UnlessStatement(ToLexicalInfo(u));
						us.Condition = condition;
					
			}
			compound_stmt(us.Block.Statements);
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_40_);
			}
			else
			{
				throw;
			}
		}
		return us;
	}
	
	protected TryStatement  try_stmt() //throws RecognitionException, TokenStreamException
{
		TryStatement s;
		
		Token  t = null;
		Token  stoken = null;
		Token  etoken = null;
		
				s = null;		
				Block sblock = null;
				Block eblock = null;
			
		
		try {      // for error handling
			t = LT(1);
			match(TRY);
			if (0==inputState.guessing)
			{
				s = new TryStatement(ToLexicalInfo(t));
			}
			compound_stmt(s.ProtectedBlock.Statements);
			{    // ( ... )*
				for (;;)
				{
					if ((LA(1)==EXCEPT))
					{
						exception_handler(s);
					}
					else
					{
						goto _loop148_breakloop;
					}
					
				}
_loop148_breakloop:				;
			}    // ( ... )*
			{
				switch ( LA(1) )
				{
				case SUCCESS:
				{
					stoken = LT(1);
					match(SUCCESS);
					if (0==inputState.guessing)
					{
						sblock = new Block(ToLexicalInfo(stoken));
					}
					compound_stmt(sblock.Statements);
					if (0==inputState.guessing)
					{
						s.SuccessBlock = sblock;
					}
					break;
				}
				case EOF:
				case TIMESPAN:
				case DOUBLE:
				case LONG:
				case DEDENT:
				case BREAK:
				case CONTINUE:
				case CAST:
				case ENSURE:
				case FOR:
				case FALSE:
				case GIVEN:
				case IF:
				case NOT:
				case NULL:
				case RAISE:
				case RETURN:
				case RETRY:
				case SELF:
				case SUPER:
				case TRY:
				case TRUE:
				case TYPEOF:
				case UNLESS:
				case WHILE:
				case YIELD:
				case TRIPLE_QUOTED_STRING:
				case ID:
				case LBRACK:
				case LPAREN:
				case SUBTRACT:
				case INCREMENT:
				case DECREMENT:
				case INT:
				case DOUBLE_QUOTED_STRING:
				case SINGLE_QUOTED_STRING:
				case LBRACE:
				case RE_LITERAL:
				{
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			{
				switch ( LA(1) )
				{
				case ENSURE:
				{
					etoken = LT(1);
					match(ENSURE);
					if (0==inputState.guessing)
					{
						eblock = new Block(ToLexicalInfo(etoken));
					}
					compound_stmt(eblock.Statements);
					if (0==inputState.guessing)
					{
						s.EnsureBlock = eblock;
					}
					break;
				}
				case EOF:
				case TIMESPAN:
				case DOUBLE:
				case LONG:
				case DEDENT:
				case BREAK:
				case CONTINUE:
				case CAST:
				case FOR:
				case FALSE:
				case GIVEN:
				case IF:
				case NOT:
				case NULL:
				case RAISE:
				case RETURN:
				case RETRY:
				case SELF:
				case SUPER:
				case TRY:
				case TRUE:
				case TYPEOF:
				case UNLESS:
				case WHILE:
				case YIELD:
				case TRIPLE_QUOTED_STRING:
				case ID:
				case LBRACK:
				case LPAREN:
				case SUBTRACT:
				case INCREMENT:
				case DECREMENT:
				case INT:
				case DOUBLE_QUOTED_STRING:
				case SINGLE_QUOTED_STRING:
				case LBRACE:
				case RE_LITERAL:
				{
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_40_);
			}
			else
			{
				throw;
			}
		}
		return s;
	}
	
	protected GivenStatement  given_stmt() //throws RecognitionException, TokenStreamException
{
		GivenStatement gs;
		
		Token  given = null;
		Token  when = null;
		Token  otherwise = null;
		
				gs = null;		
				Expression e = null;
				WhenClause wc = null;
			
		
		try {      // for error handling
			given = LT(1);
			match(GIVEN);
			e=expression();
			if (0==inputState.guessing)
			{
				
						gs = new GivenStatement(ToLexicalInfo(given));
						gs.Expression = e;
					
			}
			begin();
			{ // ( ... )+
			int _cnt167=0;
			for (;;)
			{
				if ((LA(1)==WHEN))
				{
					when = LT(1);
					match(WHEN);
					e=tuple_or_expression();
					if (0==inputState.guessing)
					{
						
										wc = new WhenClause(ToLexicalInfo(when));
										wc.Condition = e;
										gs.WhenClauses.Add(wc);
									
					}
					compound_stmt(wc.Block.Statements);
				}
				else
				{
					if (_cnt167 >= 1) { goto _loop167_breakloop; } else { throw new NoViableAltException(LT(1), getFilename());; }
				}
				
				_cnt167++;
			}
_loop167_breakloop:			;
			}    // ( ... )+
			{
				switch ( LA(1) )
				{
				case OTHERWISE:
				{
					otherwise = LT(1);
					match(OTHERWISE);
					if (0==inputState.guessing)
					{
						
										gs.OtherwiseBlock = new Block(ToLexicalInfo(otherwise));
									
					}
					compound_stmt(gs.OtherwiseBlock.Statements);
					break;
				}
				case DEDENT:
				{
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			end();
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_40_);
			}
			else
			{
				throw;
			}
		}
		return gs;
	}
	
	protected ReturnStatement  return_stmt() //throws RecognitionException, TokenStreamException
{
		ReturnStatement s;
		
		Token  r = null;
		
				s = null;
				Expression e = null;
			
		
		try {      // for error handling
			r = LT(1);
			match(RETURN);
			{
				switch ( LA(1) )
				{
				case TIMESPAN:
				case DOUBLE:
				case LONG:
				case CAST:
				case FALSE:
				case NOT:
				case NULL:
				case SELF:
				case SUPER:
				case TRUE:
				case TYPEOF:
				case TRIPLE_QUOTED_STRING:
				case ID:
				case LBRACK:
				case COMMA:
				case LPAREN:
				case SUBTRACT:
				case INCREMENT:
				case DECREMENT:
				case INT:
				case DOUBLE_QUOTED_STRING:
				case SINGLE_QUOTED_STRING:
				case LBRACE:
				case RE_LITERAL:
				{
					e=tuple_or_expression();
					break;
				}
				case IF:
				case UNLESS:
				case WHILE:
				case EOS:
				{
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			if (0==inputState.guessing)
			{
				
						s = new ReturnStatement(ToLexicalInfo(r));
						s.Expression = e;
					
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_43_);
			}
			else
			{
				throw;
			}
		}
		return s;
	}
	
	protected YieldStatement  yield_stmt() //throws RecognitionException, TokenStreamException
{
		YieldStatement s;
		
		Token  yt = null;
		
				s = null;
				Expression e = null;
			
		
		try {      // for error handling
			yt = LT(1);
			match(YIELD);
			e=tuple_or_expression();
			if (0==inputState.guessing)
			{
				
						s = new YieldStatement(ToLexicalInfo(yt));
						s.Expression = e;
					
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_43_);
			}
			else
			{
				throw;
			}
		}
		return s;
	}
	
	protected BreakStatement  break_stmt() //throws RecognitionException, TokenStreamException
{
		BreakStatement s;
		
		Token  b = null;
		s = null;
		
		try {      // for error handling
			b = LT(1);
			match(BREAK);
			if (0==inputState.guessing)
			{
				s = new BreakStatement(ToLexicalInfo(b));
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_43_);
			}
			else
			{
				throw;
			}
		}
		return s;
	}
	
	protected Statement  continue_stmt() //throws RecognitionException, TokenStreamException
{
		Statement s;
		
		Token  c = null;
		s = null;
		
		try {      // for error handling
			c = LT(1);
			match(CONTINUE);
			if (0==inputState.guessing)
			{
				s = new ContinueStatement(ToLexicalInfo(c));
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_43_);
			}
			else
			{
				throw;
			}
		}
		return s;
	}
	
	protected RaiseStatement  raise_stmt() //throws RecognitionException, TokenStreamException
{
		RaiseStatement s;
		
		Token  t = null;
		
				s = null;
				Expression e = null;
			
		
		try {      // for error handling
			t = LT(1);
			match(RAISE);
			e=expression();
			if (0==inputState.guessing)
			{
				
						s = new RaiseStatement(ToLexicalInfo(t));
						s.Exception = e;
					
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_43_);
			}
			else
			{
				throw;
			}
		}
		return s;
	}
	
	protected RetryStatement  retry_stmt() //throws RecognitionException, TokenStreamException
{
		RetryStatement rs;
		
		Token  t = null;
		
				rs = null;
			
		
		try {      // for error handling
			t = LT(1);
			match(RETRY);
			if (0==inputState.guessing)
			{
				rs = new RetryStatement(ToLexicalInfo(t));
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_43_);
			}
			else
			{
				throw;
			}
		}
		return rs;
	}
	
	protected Declaration  declaration() //throws RecognitionException, TokenStreamException
{
		Declaration d;
		
		Token  id = null;
		
				d = null;
				TypeReference tr = null;
			
		
		try {      // for error handling
			id = LT(1);
			match(ID);
			{
				switch ( LA(1) )
				{
				case AS:
				{
					match(AS);
					tr=type_reference();
					break;
				}
				case IN:
				case ASSIGN:
				case COMMA:
				{
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			if (0==inputState.guessing)
			{
				
						d = new Declaration(ToLexicalInfo(id));
						d.Name = id.getText();
						d.Type = tr;
					
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_44_);
			}
			else
			{
				throw;
			}
		}
		return d;
	}
	
	protected UnpackStatement  unpack_stmt() //throws RecognitionException, TokenStreamException
{
		UnpackStatement s;
		
		Token  t = null;
		
				s = new UnpackStatement();
				Expression e = null;
			
		
		try {      // for error handling
			declaration_list(s.Declarations);
			t = LT(1);
			match(ASSIGN);
			e=tuple_or_expression();
			if (0==inputState.guessing)
			{
				
						s.Expression = e;
						s.LexicalInfo = ToLexicalInfo(t);
					
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_43_);
			}
			else
			{
				throw;
			}
		}
		return s;
	}
	
	protected DeclarationStatement  declaration_stmt() //throws RecognitionException, TokenStreamException
{
		DeclarationStatement s;
		
		Token  id = null;
		
				s = null;
				TypeReference tr = null;
				Expression initializer = null;
			
		
		try {      // for error handling
			id = LT(1);
			match(ID);
			match(AS);
			tr=type_reference();
			{
				switch ( LA(1) )
				{
				case ASSIGN:
				{
					match(ASSIGN);
					initializer=tuple_or_expression();
					break;
				}
				case IF:
				case UNLESS:
				case WHILE:
				case EOS:
				{
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			if (0==inputState.guessing)
			{
				
						Declaration d = new Declaration(ToLexicalInfo(id));
						d.Name = id.getText();
						d.Type = tr;
						
						s = new DeclarationStatement(d.LexicalInfo);
						s.Declaration = d;
						s.Initializer = initializer;
					
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_43_);
			}
			else
			{
				throw;
			}
		}
		return s;
	}
	
	protected Expression  slicing_expression() //throws RecognitionException, TokenStreamException
{
		Expression e;
		
		Token  lbrack = null;
		Token  id = null;
		Token  lparen = null;
		
				e = null;
				Expression begin = null;
				Expression end = null;
				Expression step = null;		
				MethodInvocationExpression mce = null;
			
		
		try {      // for error handling
			e=atom();
			{    // ( ... )*
				for (;;)
				{
					if ((LA(1)==LBRACK) && (tokenSet_45_.member(LA(2))))
					{
						{
							lbrack = LT(1);
							match(LBRACK);
							{
								switch ( LA(1) )
								{
								case COLON:
								{
									{
										match(COLON);
										if (0==inputState.guessing)
										{
											begin = OmittedExpression.Default;
										}
										{
											switch ( LA(1) )
											{
											case TIMESPAN:
											case DOUBLE:
											case LONG:
											case CAST:
											case FALSE:
											case NOT:
											case NULL:
											case SELF:
											case SUPER:
											case TRUE:
											case TYPEOF:
											case TRIPLE_QUOTED_STRING:
											case ID:
											case LBRACK:
											case LPAREN:
											case SUBTRACT:
											case INCREMENT:
											case DECREMENT:
											case INT:
											case DOUBLE_QUOTED_STRING:
											case SINGLE_QUOTED_STRING:
											case LBRACE:
											case RE_LITERAL:
											{
												end=expression();
												break;
											}
											case COLON:
											{
												{
													match(COLON);
													if (0==inputState.guessing)
													{
														end = OmittedExpression.Default;
													}
													step=expression();
												}
												break;
											}
											case RBRACK:
											{
												break;
											}
											default:
											{
												throw new NoViableAltException(LT(1), getFilename());
											}
											 }
										}
									}
									break;
								}
								case TIMESPAN:
								case DOUBLE:
								case LONG:
								case CAST:
								case FALSE:
								case NOT:
								case NULL:
								case SELF:
								case SUPER:
								case TRUE:
								case TYPEOF:
								case TRIPLE_QUOTED_STRING:
								case ID:
								case LBRACK:
								case LPAREN:
								case SUBTRACT:
								case INCREMENT:
								case DECREMENT:
								case INT:
								case DOUBLE_QUOTED_STRING:
								case SINGLE_QUOTED_STRING:
								case LBRACE:
								case RE_LITERAL:
								{
									begin=expression();
									{
										switch ( LA(1) )
										{
										case COLON:
										{
											match(COLON);
											{
												switch ( LA(1) )
												{
												case TIMESPAN:
												case DOUBLE:
												case LONG:
												case CAST:
												case FALSE:
												case NOT:
												case NULL:
												case SELF:
												case SUPER:
												case TRUE:
												case TYPEOF:
												case TRIPLE_QUOTED_STRING:
												case ID:
												case LBRACK:
												case LPAREN:
												case SUBTRACT:
												case INCREMENT:
												case DECREMENT:
												case INT:
												case DOUBLE_QUOTED_STRING:
												case SINGLE_QUOTED_STRING:
												case LBRACE:
												case RE_LITERAL:
												{
													end=expression();
													break;
												}
												case RBRACK:
												case COLON:
												{
													if (0==inputState.guessing)
													{
														end = OmittedExpression.Default;
													}
													break;
												}
												default:
												{
													throw new NoViableAltException(LT(1), getFilename());
												}
												 }
											}
											{
												switch ( LA(1) )
												{
												case COLON:
												{
													match(COLON);
													step=expression();
													break;
												}
												case RBRACK:
												{
													break;
												}
												default:
												{
													throw new NoViableAltException(LT(1), getFilename());
												}
												 }
											}
											break;
										}
										case RBRACK:
										{
											break;
										}
										default:
										{
											throw new NoViableAltException(LT(1), getFilename());
										}
										 }
									}
									break;
								}
								default:
								{
									throw new NoViableAltException(LT(1), getFilename());
								}
								 }
							}
							if (0==inputState.guessing)
							{
								
												SlicingExpression se = new SlicingExpression(ToLexicalInfo(lbrack));
												se.Target = e;
												se.Begin = begin;
												se.End = end;
												se.Step = step;
												e = se;
												
												begin = end = step = null;
											
							}
							match(RBRACK);
						}
					}
					else if ((LA(1)==DOT) && (LA(2)==ID)) {
						{
							match(DOT);
							id = LT(1);
							match(ID);
							if (0==inputState.guessing)
							{
								
													MemberReferenceExpression mre = new MemberReferenceExpression(ToLexicalInfo(id));
													mre.Target = e;
													mre.Name = id.getText();
													e = mre;
												
							}
						}
					}
					else if ((LA(1)==LPAREN) && (tokenSet_46_.member(LA(2)))) {
						{
							lparen = LT(1);
							match(LPAREN);
							if (0==inputState.guessing)
							{
								
													mce = new MethodInvocationExpression(ToLexicalInfo(lparen));
													mce.Target = e;
													e = mce;
												
							}
							parameter_list(mce);
							match(RPAREN);
						}
					}
					else
					{
						goto _loop252_breakloop;
					}
					
				}
_loop252_breakloop:				;
			}    // ( ... )*
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return e;
	}
	
	protected Statement  assignment_stmt() //throws RecognitionException, TokenStreamException
{
		Statement stmt;
		
		Token  op = null;
		
				stmt = null;
				Expression lhs = null;
				Expression rhs = null;		
			
		
		try {      // for error handling
			lhs=slicing_expression();
			op = LT(1);
			match(ASSIGN);
			rhs=tuple_or_expression();
			if (0==inputState.guessing)
			{
				
						stmt = new ExpressionStatement(
											new BinaryExpression(ToLexicalInfo(op),
												ParseAssignOperator(op.getText()),
												lhs, rhs));
					
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_43_);
			}
			else
			{
				throw;
			}
		}
		return stmt;
	}
	
	protected ExpressionStatement  expression_stmt() //throws RecognitionException, TokenStreamException
{
		ExpressionStatement s;
		
		
				s = null;
				Expression e = null;
			
		
		try {      // for error handling
			e=expression();
			if (0==inputState.guessing)
			{
				
						s = new ExpressionStatement(e);
					
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_43_);
			}
			else
			{
				throw;
			}
		}
		return s;
	}
	
	protected StatementModifier  stmt_modifier() //throws RecognitionException, TokenStreamException
{
		StatementModifier m;
		
		Token  i = null;
		Token  u = null;
		Token  w = null;
		
				m = null;
				Expression e = null;
				Token t = null;
				StatementModifierType type = StatementModifierType.Uninitialized;
			
		
		try {      // for error handling
			{
				switch ( LA(1) )
				{
				case IF:
				{
					i = LT(1);
					match(IF);
					if (0==inputState.guessing)
					{
						t = i; type = StatementModifierType.If;
					}
					break;
				}
				case UNLESS:
				{
					u = LT(1);
					match(UNLESS);
					if (0==inputState.guessing)
					{
						t = u; type = StatementModifierType.Unless;
					}
					break;
				}
				case WHILE:
				{
					w = LT(1);
					match(WHILE);
					if (0==inputState.guessing)
					{
						t = w; type = StatementModifierType.While;
					}
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			e=expression();
			if (0==inputState.guessing)
			{
				
						m = new StatementModifier(ToLexicalInfo(t));
						m.Type = type;
						m.Condition = e;
					
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return m;
	}
	
	protected void exception_handler(
		TryStatement t
	) //throws RecognitionException, TokenStreamException
{
		
		Token  c = null;
		Token  x = null;
		
				ExceptionHandler eh = null;		
				TypeReference tr = null;
			
		
		try {      // for error handling
			c = LT(1);
			match(EXCEPT);
			x = LT(1);
			match(ID);
			{
				switch ( LA(1) )
				{
				case AS:
				{
					match(AS);
					tr=type_reference();
					break;
				}
				case COLON:
				{
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			if (0==inputState.guessing)
			{
				
						eh = new ExceptionHandler(ToLexicalInfo(c));
						eh.Declaration = new Declaration(ToLexicalInfo(x));
						eh.Declaration.Name = x.getText();		
						eh.Declaration.Type = tr;
					
			}
			compound_stmt(eh.Block.Statements);
			if (0==inputState.guessing)
			{
				
						t.ExceptionHandlers.Add(eh);
					
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_47_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected Expression  tuple_or_expression() //throws RecognitionException, TokenStreamException
{
		Expression e;
		
		Token  c = null;
		Token  t = null;
		
				e = null;
				TupleLiteralExpression tle = null;
			
		
		try {      // for error handling
			switch ( LA(1) )
			{
			case COMMA:
			{
				{
					c = LT(1);
					match(COMMA);
					if (0==inputState.guessing)
					{
						e = new TupleLiteralExpression(ToLexicalInfo(c));
					}
				}
				break;
			}
			case TIMESPAN:
			case DOUBLE:
			case LONG:
			case CAST:
			case FALSE:
			case NOT:
			case NULL:
			case SELF:
			case SUPER:
			case TRUE:
			case TYPEOF:
			case TRIPLE_QUOTED_STRING:
			case ID:
			case LBRACK:
			case LPAREN:
			case SUBTRACT:
			case INCREMENT:
			case DECREMENT:
			case INT:
			case DOUBLE_QUOTED_STRING:
			case SINGLE_QUOTED_STRING:
			case LBRACE:
			case RE_LITERAL:
			{
				{
					e=expression();
					{
						if ((LA(1)==COMMA) && (tokenSet_48_.member(LA(2))))
						{
							t = LT(1);
							match(COMMA);
							if (0==inputState.guessing)
							{
													
													tle = new TupleLiteralExpression(e.LexicalInfo);
													tle.Items.Add(e);		
												
							}
							{
								if ((tokenSet_33_.member(LA(1))) && (tokenSet_48_.member(LA(2))))
								{
									e=expression();
									if (0==inputState.guessing)
									{
										tle.Items.Add(e);
									}
									{    // ( ... )*
										for (;;)
										{
											if ((LA(1)==COMMA) && (tokenSet_33_.member(LA(2))))
											{
												match(COMMA);
												e=expression();
												if (0==inputState.guessing)
												{
													tle.Items.Add(e);
												}
											}
											else
											{
												goto _loop183_breakloop;
											}
											
										}
_loop183_breakloop:										;
									}    // ( ... )*
								}
								else if ((tokenSet_18_.member(LA(1))) && (tokenSet_32_.member(LA(2)))) {
								}
								else
								{
									throw new NoViableAltException(LT(1), getFilename());
								}
								
							}
							if (0==inputState.guessing)
							{
								
													e = tle;
												
							}
						}
						else if ((tokenSet_18_.member(LA(1))) && (tokenSet_32_.member(LA(2)))) {
						}
						else
						{
							throw new NoViableAltException(LT(1), getFilename());
						}
						
					}
				}
				break;
			}
			default:
			{
				throw new NoViableAltException(LT(1), getFilename());
			}
			 }
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return e;
	}
	
	protected void declaration_list(
		DeclarationCollection dc
	) //throws RecognitionException, TokenStreamException
{
		
		
				Declaration d = null;
			
		
		try {      // for error handling
			d=declaration();
			if (0==inputState.guessing)
			{
				dc.Add(d);
			}
			{    // ( ... )*
				for (;;)
				{
					if ((LA(1)==COMMA))
					{
						match(COMMA);
						d=declaration();
						if (0==inputState.guessing)
						{
							dc.Add(d);
						}
					}
					else
					{
						goto _loop174_breakloop;
					}
					
				}
_loop174_breakloop:				;
			}    // ( ... )*
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_49_);
			}
			else
			{
				throw;
			}
		}
	}
	
	protected Expression  boolean_expression() //throws RecognitionException, TokenStreamException
{
		Expression e;
		
		Token  nt = null;
		Token  ot = null;
		
				e = null;
				Expression r = null;
			
		
		try {      // for error handling
			switch ( LA(1) )
			{
			case NOT:
			{
				{
					nt = LT(1);
					match(NOT);
					e=boolean_expression();
					if (0==inputState.guessing)
					{
						
									UnaryExpression ue = new UnaryExpression(ToLexicalInfo(nt));
									ue.Operator = UnaryOperatorType.LogicalNot;
									ue.Operand = e;
									e = ue;
								
					}
				}
				break;
			}
			case TIMESPAN:
			case DOUBLE:
			case LONG:
			case CAST:
			case FALSE:
			case NULL:
			case SELF:
			case SUPER:
			case TRUE:
			case TYPEOF:
			case TRIPLE_QUOTED_STRING:
			case ID:
			case LBRACK:
			case LPAREN:
			case SUBTRACT:
			case INCREMENT:
			case DECREMENT:
			case INT:
			case DOUBLE_QUOTED_STRING:
			case SINGLE_QUOTED_STRING:
			case LBRACE:
			case RE_LITERAL:
			{
				{
					e=boolean_term();
					{    // ( ... )*
						for (;;)
						{
							if ((LA(1)==OR) && (tokenSet_33_.member(LA(2))))
							{
								ot = LT(1);
								match(OR);
								r=expression();
								if (0==inputState.guessing)
								{
									
													BinaryExpression be = new BinaryExpression(ToLexicalInfo(ot));
													be.Operator = BinaryOperatorType.Or;
													be.Left = e;
													be.Right = r;
													e = be;
												
								}
							}
							else
							{
								goto _loop192_breakloop;
							}
							
						}
_loop192_breakloop:						;
					}    // ( ... )*
				}
				break;
			}
			default:
			{
				throw new NoViableAltException(LT(1), getFilename());
			}
			 }
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return e;
	}
	
	protected Expression  boolean_term() //throws RecognitionException, TokenStreamException
{
		Expression e;
		
		Token  at = null;
		
				e = null;
				Expression r = null;
			
		
		try {      // for error handling
			e=ternary_expression();
			{    // ( ... )*
				for (;;)
				{
					if ((LA(1)==AND) && (tokenSet_33_.member(LA(2))))
					{
						at = LT(1);
						match(AND);
						r=expression();
						if (0==inputState.guessing)
						{
							
										BinaryExpression be = new BinaryExpression(ToLexicalInfo(at));
										be.Operator = BinaryOperatorType.And;
										be.Left = e;
										be.Right = r; 
										e = be;
									
						}
					}
					else
					{
						goto _loop195_breakloop;
					}
					
				}
_loop195_breakloop:				;
			}    // ( ... )*
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return e;
	}
	
	protected Expression  ternary_expression() //throws RecognitionException, TokenStreamException
{
		Expression e;
		
		Token  t = null;
		
				e = null;			
				Expression te = null;
				Expression fe = null;
			
		
		try {      // for error handling
			e=assignment_expression();
			{    // ( ... )*
				for (;;)
				{
					if ((LA(1)==QMARK) && (tokenSet_50_.member(LA(2))))
					{
						t = LT(1);
						match(QMARK);
						te=ternary_expression();
						match(COLON);
						fe=assignment_expression();
						if (0==inputState.guessing)
						{
							
										TernaryExpression finalExpression = new TernaryExpression(ToLexicalInfo(t));
										finalExpression.Condition = e;
										finalExpression.TrueExpression = te;
										finalExpression.FalseExpression = fe;
										e = finalExpression;
									
						}
					}
					else
					{
						goto _loop198_breakloop;
					}
					
				}
_loop198_breakloop:				;
			}    // ( ... )*
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return e;
	}
	
	protected Expression  assignment_expression() //throws RecognitionException, TokenStreamException
{
		Expression e;
		
		Token  op = null;
		
				e = null;
				Expression r=null;
			
		
		try {      // for error handling
			e=conditional_expression();
			{
				if ((LA(1)==ASSIGN) && (tokenSet_50_.member(LA(2))))
				{
					op = LT(1);
					match(ASSIGN);
					r=conditional_expression();
					if (0==inputState.guessing)
					{
						
									BinaryExpression be = new BinaryExpression(ToLexicalInfo(op));
									be.Operator = ParseAssignOperator(op.getText());
									be.Left = e;
									be.Right = r;
									e = be; 
								
					}
				}
				else if ((tokenSet_18_.member(LA(1))) && (tokenSet_32_.member(LA(2)))) {
				}
				else
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return e;
	}
	
	protected Expression  conditional_expression() //throws RecognitionException, TokenStreamException
{
		Expression e;
		
		Token  t = null;
		Token  tis = null;
		Token  tisa = null;
		Token  tin = null;
		Token  tnint = null;
		
				e = null;		
				Expression r = null;
				BinaryOperatorType op = BinaryOperatorType.None;
				Token token = null;
			
		
		try {      // for error handling
			e=sum();
			{    // ( ... )*
				for (;;)
				{
					if ((tokenSet_51_.member(LA(1))) && (tokenSet_52_.member(LA(2))))
					{
						{
							switch ( LA(1) )
							{
							case IS:
							case ISA:
							case CMP_OPERATOR:
							{
								{
									{
										switch ( LA(1) )
										{
										case CMP_OPERATOR:
										{
											{
												t = LT(1);
												match(CMP_OPERATOR);
												if (0==inputState.guessing)
												{
													op = ParseCmpOperator(t.getText()); token = t;
												}
											}
											break;
										}
										case IS:
										{
											{
												tis = LT(1);
												match(IS);
												if (0==inputState.guessing)
												{
													op = BinaryOperatorType.ReferenceEquality; token = tis;
												}
												{
													switch ( LA(1) )
													{
													case NOT:
													{
														match(NOT);
														if (0==inputState.guessing)
														{
															op = BinaryOperatorType.ReferenceInequality;
														}
														break;
													}
													case TIMESPAN:
													case DOUBLE:
													case LONG:
													case CAST:
													case FALSE:
													case NULL:
													case SELF:
													case SUPER:
													case TRUE:
													case TYPEOF:
													case TRIPLE_QUOTED_STRING:
													case ID:
													case LBRACK:
													case LPAREN:
													case SUBTRACT:
													case INCREMENT:
													case DECREMENT:
													case INT:
													case DOUBLE_QUOTED_STRING:
													case SINGLE_QUOTED_STRING:
													case LBRACE:
													case RE_LITERAL:
													{
														break;
													}
													default:
													{
														throw new NoViableAltException(LT(1), getFilename());
													}
													 }
												}
											}
											break;
										}
										case ISA:
										{
											{
												tisa = LT(1);
												match(ISA);
												if (0==inputState.guessing)
												{
													op = BinaryOperatorType.TypeTest; token = tisa;
												}
											}
											break;
										}
										default:
										{
											throw new NoViableAltException(LT(1), getFilename());
										}
										 }
									}
									r=sum();
								}
								break;
							}
							case IN:
							case NOT:
							{
								{
									{
										switch ( LA(1) )
										{
										case IN:
										{
											{
												tin = LT(1);
												match(IN);
												if (0==inputState.guessing)
												{
													op = BinaryOperatorType.Member; token = tin;
												}
											}
											break;
										}
										case NOT:
										{
											{
												tnint = LT(1);
												match(NOT);
												match(IN);
												if (0==inputState.guessing)
												{
													op = BinaryOperatorType.NotMember; token = tnint;
												}
											}
											break;
										}
										default:
										{
											throw new NoViableAltException(LT(1), getFilename());
										}
										 }
									}
									r=tuple_or_expression();
								}
								break;
							}
							default:
							{
								throw new NoViableAltException(LT(1), getFilename());
							}
							 }
						}
						if (0==inputState.guessing)
						{
							
									BinaryExpression be = new BinaryExpression(ToLexicalInfo(token));
									be.Operator = op;
									be.Left = e;
									be.Right = r;
									e = be;
								
						}
					}
					else
					{
						goto _loop215_breakloop;
					}
					
				}
_loop215_breakloop:				;
			}    // ( ... )*
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return e;
	}
	
	protected Expression  sum() //throws RecognitionException, TokenStreamException
{
		Expression e;
		
		Token  add = null;
		Token  sub = null;
		Token  bitor = null;
		
				e = null;
				Expression r = null;
				Token op = null;
				BinaryOperatorType bOperator = BinaryOperatorType.None;
			
		
		try {      // for error handling
			e=term();
			{    // ( ... )*
				for (;;)
				{
					if (((LA(1) >= ADD && LA(1) <= BITWISE_OR)) && (tokenSet_50_.member(LA(2))))
					{
						{
							switch ( LA(1) )
							{
							case ADD:
							{
								add = LT(1);
								match(ADD);
								if (0==inputState.guessing)
								{
									op=add; bOperator = BinaryOperatorType.Addition;
								}
								break;
							}
							case SUBTRACT:
							{
								sub = LT(1);
								match(SUBTRACT);
								if (0==inputState.guessing)
								{
									op=sub; bOperator = BinaryOperatorType.Subtraction;
								}
								break;
							}
							case BITWISE_OR:
							{
								bitor = LT(1);
								match(BITWISE_OR);
								if (0==inputState.guessing)
								{
									op=bitor; bOperator = BinaryOperatorType.BitwiseOr;
								}
								break;
							}
							default:
							{
								throw new NoViableAltException(LT(1), getFilename());
							}
							 }
						}
						r=term();
						if (0==inputState.guessing)
						{
							
										BinaryExpression be = new BinaryExpression(ToLexicalInfo(op));
										be.Operator = bOperator;
										be.Left = e;
										be.Right = r;
										e = be;
									
						}
					}
					else
					{
						goto _loop219_breakloop;
					}
					
				}
_loop219_breakloop:				;
			}    // ( ... )*
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return e;
	}
	
	protected Expression  term() //throws RecognitionException, TokenStreamException
{
		Expression e;
		
		Token  m = null;
		Token  d = null;
		Token  md = null;
		
				e = null;
				Expression r = null;
				Token token = null;
				BinaryOperatorType op = BinaryOperatorType.None; 
			
		
		try {      // for error handling
			e=exponentiation();
			{    // ( ... )*
				for (;;)
				{
					if (((LA(1) >= MULTIPLY && LA(1) <= MODULUS)) && (tokenSet_50_.member(LA(2))))
					{
						{
							switch ( LA(1) )
							{
							case MULTIPLY:
							{
								m = LT(1);
								match(MULTIPLY);
								if (0==inputState.guessing)
								{
									op=BinaryOperatorType.Multiply; token=m;
								}
								break;
							}
							case DIVISION:
							{
								d = LT(1);
								match(DIVISION);
								if (0==inputState.guessing)
								{
									op=BinaryOperatorType.Division; token=d;
								}
								break;
							}
							case MODULUS:
							{
								md = LT(1);
								match(MODULUS);
								if (0==inputState.guessing)
								{
									op=BinaryOperatorType.Modulus; token=md;
								}
								break;
							}
							default:
							{
								throw new NoViableAltException(LT(1), getFilename());
							}
							 }
						}
						r=exponentiation();
						if (0==inputState.guessing)
						{
							
										BinaryExpression be = new BinaryExpression(ToLexicalInfo(token));
										be.Operator = op;
										be.Left = e;
										be.Right = r;
										e = be;
									
						}
					}
					else
					{
						goto _loop223_breakloop;
					}
					
				}
_loop223_breakloop:				;
			}    // ( ... )*
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return e;
	}
	
	protected Expression  exponentiation() //throws RecognitionException, TokenStreamException
{
		Expression e;
		
		Token  token = null;
		
				e = null;
				Expression r = null;
			
		
		try {      // for error handling
			e=unary_expression();
			{    // ( ... )*
				for (;;)
				{
					if ((LA(1)==EXPONENTIATION) && (tokenSet_50_.member(LA(2))))
					{
						token = LT(1);
						match(EXPONENTIATION);
						r=exponentiation();
						if (0==inputState.guessing)
						{
							
										BinaryExpression be = new BinaryExpression(ToLexicalInfo(token));
										be.Operator = BinaryOperatorType.Exponentiation;
										be.Left = e;
										be.Right = r;
										e = be;
									
						}
					}
					else
					{
						goto _loop226_breakloop;
					}
					
				}
_loop226_breakloop:				;
			}    // ( ... )*
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return e;
	}
	
	protected Expression  unary_expression() //throws RecognitionException, TokenStreamException
{
		Expression e;
		
		Token  sub = null;
		Token  inc = null;
		Token  dec = null;
		
					e = null;
					Token op = null;
					UnaryOperatorType uOperator = UnaryOperatorType.None;
			
		
		try {      // for error handling
			{
				switch ( LA(1) )
				{
				case SUBTRACT:
				{
					sub = LT(1);
					match(SUBTRACT);
					if (0==inputState.guessing)
					{
						op = sub; uOperator = UnaryOperatorType.UnaryNegation;
					}
					break;
				}
				case INCREMENT:
				{
					inc = LT(1);
					match(INCREMENT);
					if (0==inputState.guessing)
					{
						op = inc; uOperator = UnaryOperatorType.Increment;
					}
					break;
				}
				case DECREMENT:
				{
					dec = LT(1);
					match(DECREMENT);
					if (0==inputState.guessing)
					{
						op = dec; uOperator = UnaryOperatorType.Decrement;
					}
					break;
				}
				case TIMESPAN:
				case DOUBLE:
				case LONG:
				case CAST:
				case FALSE:
				case NULL:
				case SELF:
				case SUPER:
				case TRUE:
				case TYPEOF:
				case TRIPLE_QUOTED_STRING:
				case ID:
				case LBRACK:
				case LPAREN:
				case INT:
				case DOUBLE_QUOTED_STRING:
				case SINGLE_QUOTED_STRING:
				case LBRACE:
				case RE_LITERAL:
				{
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			e=slicing_expression();
			if (0==inputState.guessing)
			{
				
						if (null != op)
						{
							UnaryExpression ue = new UnaryExpression(ToLexicalInfo(op));
							ue.Operator = uOperator;
							ue.Operand = e;
							e = ue; 
						}
					
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return e;
	}
	
	protected Expression  atom() //throws RecognitionException, TokenStreamException
{
		Expression e;
		
		
				e = null;
			
		
		try {      // for error handling
			{
				switch ( LA(1) )
				{
				case TIMESPAN:
				case DOUBLE:
				case LONG:
				case FALSE:
				case NULL:
				case SELF:
				case SUPER:
				case TRUE:
				case TRIPLE_QUOTED_STRING:
				case LBRACK:
				case INT:
				case DOUBLE_QUOTED_STRING:
				case SINGLE_QUOTED_STRING:
				case LBRACE:
				case RE_LITERAL:
				{
					e=literal();
					break;
				}
				case ID:
				{
					e=reference_expression();
					break;
				}
				case LPAREN:
				{
					e=paren_expression();
					break;
				}
				case CAST:
				{
					e=cast_expression();
					break;
				}
				case TYPEOF:
				{
					e=typeof_expression();
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return e;
	}
	
	protected Expression  literal() //throws RecognitionException, TokenStreamException
{
		Expression e;
		
		
				e = null;
			
		
		try {      // for error handling
			{
				switch ( LA(1) )
				{
				case LONG:
				case INT:
				{
					e=integer_literal();
					break;
				}
				case TRIPLE_QUOTED_STRING:
				case DOUBLE_QUOTED_STRING:
				case SINGLE_QUOTED_STRING:
				{
					e=string_literal();
					break;
				}
				case LBRACK:
				{
					e=list_literal();
					break;
				}
				case LBRACE:
				{
					e=hash_literal();
					break;
				}
				case RE_LITERAL:
				{
					e=re_literal();
					break;
				}
				case FALSE:
				case TRUE:
				{
					e=bool_literal();
					break;
				}
				case NULL:
				{
					e=null_literal();
					break;
				}
				case SELF:
				{
					e=self_literal();
					break;
				}
				case SUPER:
				{
					e=super_literal();
					break;
				}
				case DOUBLE:
				{
					e=double_literal();
					break;
				}
				case TIMESPAN:
				{
					e=timespan_literal();
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return e;
	}
	
	protected ReferenceExpression  reference_expression() //throws RecognitionException, TokenStreamException
{
		ReferenceExpression e;
		
		Token  id = null;
		e = null;
		
		try {      // for error handling
			id = LT(1);
			match(ID);
			if (0==inputState.guessing)
			{
				
						e = new ReferenceExpression(ToLexicalInfo(id));
						e.Name = id.getText();
					
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return e;
	}
	
	protected Expression  paren_expression() //throws RecognitionException, TokenStreamException
{
		Expression e;
		
		e = null;
		
		try {      // for error handling
			match(LPAREN);
			e=tuple_or_expression();
			match(RPAREN);
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return e;
	}
	
	protected Expression  cast_expression() //throws RecognitionException, TokenStreamException
{
		Expression e;
		
		Token  t = null;
		
				e = null;
				TypeReference tr = null;
				Expression target = null;
			
		
		try {      // for error handling
			t = LT(1);
			match(CAST);
			match(LPAREN);
			tr=type_reference();
			match(COMMA);
			target=expression();
			match(RPAREN);
			if (0==inputState.guessing)
			{
				
						e = new CastExpression(ToLexicalInfo(t), tr, target);
					
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return e;
	}
	
	protected Expression  typeof_expression() //throws RecognitionException, TokenStreamException
{
		Expression e;
		
		Token  t = null;
		
				e = null;
				TypeReference tr = null;
			
		
		try {      // for error handling
			t = LT(1);
			match(TYPEOF);
			match(LPAREN);
			tr=type_reference();
			match(RPAREN);
			if (0==inputState.guessing)
			{
				
						e = new TypeofExpression(ToLexicalInfo(t), tr);
					
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return e;
	}
	
	protected Expression  tuple() //throws RecognitionException, TokenStreamException
{
		Expression e;
		
		Token  t = null;
		
				TupleLiteralExpression tle = null;
				e = null;
			
		
		try {      // for error handling
			t = LT(1);
			match(LPAREN);
			e=expression();
			{
				switch ( LA(1) )
				{
				case COMMA:
				{
					match(COMMA);
					if (0==inputState.guessing)
					{
						
									tle = new TupleLiteralExpression(ToLexicalInfo(t));
									tle.Items.Add(e);
								
					}
					{
						switch ( LA(1) )
						{
						case TIMESPAN:
						case DOUBLE:
						case LONG:
						case CAST:
						case FALSE:
						case NOT:
						case NULL:
						case SELF:
						case SUPER:
						case TRUE:
						case TYPEOF:
						case TRIPLE_QUOTED_STRING:
						case ID:
						case LBRACK:
						case LPAREN:
						case SUBTRACT:
						case INCREMENT:
						case DECREMENT:
						case INT:
						case DOUBLE_QUOTED_STRING:
						case SINGLE_QUOTED_STRING:
						case LBRACE:
						case RE_LITERAL:
						{
							e=expression();
							if (0==inputState.guessing)
							{
								tle.Items.Add(e);
							}
							{    // ( ... )*
								for (;;)
								{
									if ((LA(1)==COMMA))
									{
										match(COMMA);
										e=expression();
										if (0==inputState.guessing)
										{
											tle.Items.Add(e);
										}
									}
									else
									{
										goto _loop239_breakloop;
									}
									
								}
_loop239_breakloop:								;
							}    // ( ... )*
							break;
						}
						case RPAREN:
						{
							break;
						}
						default:
						{
							throw new NoViableAltException(LT(1), getFilename());
						}
						 }
					}
					if (0==inputState.guessing)
					{
						e = tle;
					}
					break;
				}
				case RPAREN:
				{
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			match(RPAREN);
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_3_);
			}
			else
			{
				throw;
			}
		}
		return e;
	}
	
	protected Expression  string_literal() //throws RecognitionException, TokenStreamException
{
		Expression e;
		
		Token  dqs = null;
		Token  sqs = null;
		Token  tqs = null;
		
				e = null;
			
		
		try {      // for error handling
			bool synPredMatched263 = false;
			if (((LA(1)==TRIPLE_QUOTED_STRING||LA(1)==DOUBLE_QUOTED_STRING) && (tokenSet_18_.member(LA(2)))))
			{
				int _m263 = mark();
				synPredMatched263 = true;
				inputState.guessing++;
				try {
					{
						{
							switch ( LA(1) )
							{
							case DOUBLE_QUOTED_STRING:
							{
								match(DOUBLE_QUOTED_STRING);
								break;
							}
							case TRIPLE_QUOTED_STRING:
							{
								match(TRIPLE_QUOTED_STRING);
								break;
							}
							default:
							{
								throw new NoViableAltException(LT(1), getFilename());
							}
							 }
						}
						match(ESEPARATOR);
					}
				}
				catch (RecognitionException)
				{
					synPredMatched263 = false;
				}
				rewind(_m263);
				inputState.guessing--;
			}
			if ( synPredMatched263 )
			{
				e=string_formatting();
			}
			else if ((LA(1)==DOUBLE_QUOTED_STRING) && (tokenSet_18_.member(LA(2)))) {
				dqs = LT(1);
				match(DOUBLE_QUOTED_STRING);
				if (0==inputState.guessing)
				{
					
							e = new StringLiteralExpression(ToLexicalInfo(dqs), dqs.getText());
						
				}
			}
			else if ((LA(1)==SINGLE_QUOTED_STRING)) {
				sqs = LT(1);
				match(SINGLE_QUOTED_STRING);
				if (0==inputState.guessing)
				{
					
							e = new StringLiteralExpression(ToLexicalInfo(sqs), sqs.getText());
						
				}
			}
			else if ((LA(1)==TRIPLE_QUOTED_STRING) && (tokenSet_18_.member(LA(2)))) {
				tqs = LT(1);
				match(TRIPLE_QUOTED_STRING);
				if (0==inputState.guessing)
				{
					
							e = new StringLiteralExpression(ToLexicalInfo(tqs), tqs.getText());
						
				}
			}
			else
			{
				throw new NoViableAltException(LT(1), getFilename());
			}
			
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return e;
	}
	
	protected Expression  list_literal() //throws RecognitionException, TokenStreamException
{
		Expression e;
		
		Token  lbrack = null;
		
				e = null;
				ListLiteralExpression lle = null;
				Expression item = null;
			
		
		try {      // for error handling
			lbrack = LT(1);
			match(LBRACK);
			{
				switch ( LA(1) )
				{
				case TIMESPAN:
				case DOUBLE:
				case LONG:
				case CAST:
				case FALSE:
				case NOT:
				case NULL:
				case SELF:
				case SUPER:
				case TRUE:
				case TYPEOF:
				case TRIPLE_QUOTED_STRING:
				case ID:
				case LBRACK:
				case LPAREN:
				case SUBTRACT:
				case INCREMENT:
				case DECREMENT:
				case INT:
				case DOUBLE_QUOTED_STRING:
				case SINGLE_QUOTED_STRING:
				case LBRACE:
				case RE_LITERAL:
				{
					{
						item=expression();
						{
							if (0==inputState.guessing)
							{
								
													e = lle = new ListLiteralExpression(ToLexicalInfo(lbrack));
													lle.Items.Add(item);
												
							}
							{    // ( ... )*
								for (;;)
								{
									if ((LA(1)==COMMA))
									{
										match(COMMA);
										item=expression();
										if (0==inputState.guessing)
										{
											lle.Items.Add(item);
										}
									}
									else
									{
										goto _loop273_breakloop;
									}
									
								}
_loop273_breakloop:								;
							}    // ( ... )*
						}
					}
					break;
				}
				case RBRACK:
				{
					if (0==inputState.guessing)
					{
						e = new ListLiteralExpression(ToLexicalInfo(lbrack));
					}
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			match(RBRACK);
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return e;
	}
	
	protected HashLiteralExpression  hash_literal() //throws RecognitionException, TokenStreamException
{
		HashLiteralExpression dle;
		
		Token  lbrace = null;
		
				dle = null;
				ExpressionPair pair = null;
			
		
		try {      // for error handling
			lbrace = LT(1);
			match(LBRACE);
			if (0==inputState.guessing)
			{
				dle = new HashLiteralExpression(ToLexicalInfo(lbrace));
			}
			{
				switch ( LA(1) )
				{
				case TIMESPAN:
				case DOUBLE:
				case LONG:
				case CAST:
				case FALSE:
				case NOT:
				case NULL:
				case SELF:
				case SUPER:
				case TRUE:
				case TYPEOF:
				case TRIPLE_QUOTED_STRING:
				case ID:
				case LBRACK:
				case LPAREN:
				case SUBTRACT:
				case INCREMENT:
				case DECREMENT:
				case INT:
				case DOUBLE_QUOTED_STRING:
				case SINGLE_QUOTED_STRING:
				case LBRACE:
				case RE_LITERAL:
				{
					pair=expression_pair();
					if (0==inputState.guessing)
					{
						dle.Items.Add(pair);
					}
					{    // ( ... )*
						for (;;)
						{
							if ((LA(1)==COMMA))
							{
								match(COMMA);
								pair=expression_pair();
								if (0==inputState.guessing)
								{
									dle.Items.Add(pair);
								}
							}
							else
							{
								goto _loop277_breakloop;
							}
							
						}
_loop277_breakloop:						;
					}    // ( ... )*
					break;
				}
				case RBRACE:
				{
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			match(RBRACE);
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return dle;
	}
	
	protected RELiteralExpression  re_literal() //throws RecognitionException, TokenStreamException
{
		RELiteralExpression re;
		
		Token  value = null;
		re = null;
		
		try {      // for error handling
			value = LT(1);
			match(RE_LITERAL);
			if (0==inputState.guessing)
			{
				re = new RELiteralExpression(ToLexicalInfo(value), value.getText());
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return re;
	}
	
	protected BoolLiteralExpression  bool_literal() //throws RecognitionException, TokenStreamException
{
		BoolLiteralExpression e;
		
		Token  t = null;
		Token  f = null;
		e = null;
		
		try {      // for error handling
			switch ( LA(1) )
			{
			case TRUE:
			{
				t = LT(1);
				match(TRUE);
				if (0==inputState.guessing)
				{
					
							e = new BoolLiteralExpression(ToLexicalInfo(t));
							e.Value = true;
						
				}
				break;
			}
			case FALSE:
			{
				f = LT(1);
				match(FALSE);
				if (0==inputState.guessing)
				{
					
							e = new BoolLiteralExpression(ToLexicalInfo(f));
							e.Value = false;
						
				}
				break;
			}
			default:
			{
				throw new NoViableAltException(LT(1), getFilename());
			}
			 }
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return e;
	}
	
	protected NullLiteralExpression  null_literal() //throws RecognitionException, TokenStreamException
{
		NullLiteralExpression e;
		
		Token  t = null;
		e = null;
		
		try {      // for error handling
			t = LT(1);
			match(NULL);
			if (0==inputState.guessing)
			{
				e = new NullLiteralExpression(ToLexicalInfo(t));
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return e;
	}
	
	protected SelfLiteralExpression  self_literal() //throws RecognitionException, TokenStreamException
{
		SelfLiteralExpression e;
		
		Token  t = null;
		e = null;
		
		try {      // for error handling
			t = LT(1);
			match(SELF);
			if (0==inputState.guessing)
			{
				e = new SelfLiteralExpression(ToLexicalInfo(t));
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return e;
	}
	
	protected SuperLiteralExpression  super_literal() //throws RecognitionException, TokenStreamException
{
		SuperLiteralExpression e;
		
		Token  t = null;
		e = null;
		
		try {      // for error handling
			t = LT(1);
			match(SUPER);
			if (0==inputState.guessing)
			{
				e = new SuperLiteralExpression(ToLexicalInfo(t));
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return e;
	}
	
	protected DoubleLiteralExpression  double_literal() //throws RecognitionException, TokenStreamException
{
		DoubleLiteralExpression rle;
		
		Token  value = null;
		rle = null;
		
		try {      // for error handling
			value = LT(1);
			match(DOUBLE);
			if (0==inputState.guessing)
			{
				rle = new DoubleLiteralExpression(ToLexicalInfo(value), ParseDouble(value.getText()));
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return rle;
	}
	
	protected TimeSpanLiteralExpression  timespan_literal() //throws RecognitionException, TokenStreamException
{
		TimeSpanLiteralExpression tsle;
		
		Token  value = null;
		tsle = null;
		
		try {      // for error handling
			value = LT(1);
			match(TIMESPAN);
			if (0==inputState.guessing)
			{
				tsle = new TimeSpanLiteralExpression(ToLexicalInfo(value), ParseTimeSpan(value.getText()));
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return tsle;
	}
	
	protected StringFormattingExpression  string_formatting() //throws RecognitionException, TokenStreamException
{
		StringFormattingExpression e;
		
		Token  dqs = null;
		Token  tqs = null;
		
				e = null;
				Expression param = null;
				Token stringToken = null;
			
		
		try {      // for error handling
			{
				switch ( LA(1) )
				{
				case DOUBLE_QUOTED_STRING:
				{
					dqs = LT(1);
					match(DOUBLE_QUOTED_STRING);
					if (0==inputState.guessing)
					{
						stringToken = dqs;
					}
					break;
				}
				case TRIPLE_QUOTED_STRING:
				{
					tqs = LT(1);
					match(TRIPLE_QUOTED_STRING);
					if (0==inputState.guessing)
					{
						stringToken = tqs;
					}
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			if (0==inputState.guessing)
			{
				
						e = new StringFormattingExpression(ToLexicalInfo(stringToken));
						e.Template = stringToken.getText();
					
			}
			{    // ( ... )*
				for (;;)
				{
					if ((LA(1)==ESEPARATOR) && (tokenSet_33_.member(LA(2))))
					{
						match(ESEPARATOR);
						param=expression();
						if (0==inputState.guessing)
						{
							e.Arguments.Add(param);
						}
					}
					else
					{
						goto _loop267_breakloop;
					}
					
				}
_loop267_breakloop:				;
			}    // ( ... )*
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_18_);
			}
			else
			{
				throw;
			}
		}
		return e;
	}
	
	protected ExpressionPair  expression_pair() //throws RecognitionException, TokenStreamException
{
		ExpressionPair ep;
		
		Token  t = null;
		
				ep = null;
				Expression key = null;
				Expression value = null;
			
		
		try {      // for error handling
			key=expression();
			t = LT(1);
			match(COLON);
			value=expression();
			if (0==inputState.guessing)
			{
				ep = new ExpressionPair(ToLexicalInfo(t), key, value);
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_53_);
			}
			else
			{
				throw;
			}
		}
		return ep;
	}
	
	protected void parameter(
		INodeWithArguments node
	) //throws RecognitionException, TokenStreamException
{
		
		Token  colon = null;
		
				Expression e = null;
				Expression value = null;
			
		
		try {      // for error handling
			e=expression();
			{
				switch ( LA(1) )
				{
				case COLON:
				{
					{
						colon = LT(1);
						match(COLON);
						value=expression();
						if (0==inputState.guessing)
						{
							node.NamedArguments.Add(new ExpressionPair(ToLexicalInfo(colon), e, value));
						}
					}
					break;
				}
				case COMMA:
				case RPAREN:
				{
					if (0==inputState.guessing)
					{
						node.Arguments.Add(e);
					}
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
		}
		catch (RecognitionException ex)
		{
			if (0 == inputState.guessing)
			{
				reportError(ex);
				consume();
				consumeUntil(tokenSet_41_);
			}
			else
			{
				throw;
			}
		}
	}
	
	private void initializeFactory()
	{
	}
	
	public static readonly string[] tokenNames_ = new string[] {
		@"""<0>""",
		@"""EOF""",
		@"""<2>""",
		@"""NULL_TREE_LOOKAHEAD""",
		@"""TIMESPAN""",
		@"""DOUBLE""",
		@"""LONG""",
		@"""ESEPARATOR""",
		@"""INDENT""",
		@"""DEDENT""",
		@"""COMPILATION_UNIT""",
		@"""PARAMETERS""",
		@"""PARAMETER""",
		@"""ELIST""",
		@"""DLIST""",
		@"""TYPE""",
		@"""CALL""",
		@"""STMT""",
		@"""BLOCK""",
		@"""FIELD""",
		@"""MODIFIERS""",
		@"""MODULE""",
		@"""LITERAL""",
		@"""LIST_LITERAL""",
		@"""UNPACKING""",
		@"""abstract""",
		@"""and""",
		@"""as""",
		@"""break""",
		@"""continue""",
		@"""cast""",
		@"""class""",
		@"""constructor""",
		@"""def""",
		@"""else""",
		@"""ensure""",
		@"""enum""",
		@"""except""",
		@"""failure""",
		@"""final""",
		@"""from""",
		@"""for""",
		@"""false""",
		@"""get""",
		@"""given""",
		@"""import""",
		@"""interface""",
		@"""internal""",
		@"""is""",
		@"""isa""",
		@"""if""",
		@"""in""",
		@"""not""",
		@"""null""",
		@"""or""",
		@"""otherwise""",
		@"""override""",
		@"""pass""",
		@"""namespace""",
		@"""public""",
		@"""protected""",
		@"""private""",
		@"""raise""",
		@"""return""",
		@"""retry""",
		@"""set""",
		@"""self""",
		@"""super""",
		@"""static""",
		@"""success""",
		@"""try""",
		@"""transient""",
		@"""true""",
		@"""typeof""",
		@"""unless""",
		@"""virtual""",
		@"""when""",
		@"""while""",
		@"""yield""",
		@"""EOS""",
		@"""TRIPLE_QUOTED_STRING""",
		@"""ID""",
		@"""ASSIGN""",
		@"""LBRACK""",
		@"""COMMA""",
		@"""RBRACK""",
		@"""LPAREN""",
		@"""RPAREN""",
		@"""COLON""",
		@"""QMARK""",
		@"""CMP_OPERATOR""",
		@"""ADD""",
		@"""SUBTRACT""",
		@"""BITWISE_OR""",
		@"""MULTIPLY""",
		@"""DIVISION""",
		@"""MODULUS""",
		@"""EXPONENTIATION""",
		@"""INCREMENT""",
		@"""DECREMENT""",
		@"""DOT""",
		@"""INT""",
		@"""DOUBLE_QUOTED_STRING""",
		@"""SINGLE_QUOTED_STRING""",
		@"""LBRACE""",
		@"""RBRACE""",
		@"""RE_LITERAL""",
		@"""LINE_CONTINUATION""",
		@"""SL_COMMENT""",
		@"""ML_COMMENT""",
		@"""WS""",
		@"""NEWLINE""",
		@"""ESCAPED_EXPRESSION""",
		@"""DQS_ESC""",
		@"""SQS_ESC""",
		@"""SESC""",
		@"""RE_CHAR""",
		@"""RE_ESC""",
		@"""ID_LETTER""",
		@"""DIGIT"""
	};
	
	private static long[] mk_tokenSet_0_()
	{
		long[] data = { -201264972339085198L, 6511443832797L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_0_ = new BitSet(mk_tokenSet_0_());
	private static long[] mk_tokenSet_1_()
	{
		long[] data = { 4107494595640688640L, 526480L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_1_ = new BitSet(mk_tokenSet_1_());
	private static long[] mk_tokenSet_2_()
	{
		long[] data = { 4107494599935655936L, 2230416L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_2_ = new BitSet(mk_tokenSet_2_());
	private static long[] mk_tokenSet_3_()
	{
		long[] data = { 2L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_3_ = new BitSet(mk_tokenSet_3_());
	private static long[] mk_tokenSet_4_()
	{
		long[] data = { -201264972339084430L, 6511443832797L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_4_ = new BitSet(mk_tokenSet_4_());
	private static long[] mk_tokenSet_5_()
	{
		long[] data = { -36030360420613390L, 8796084629471L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_5_ = new BitSet(mk_tokenSet_5_());
	private static long[] mk_tokenSet_6_()
	{
		long[] data = { -489495348490796942L, 6511443832797L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_6_ = new BitSet(mk_tokenSet_6_());
	private static long[] mk_tokenSet_7_()
	{
		long[] data = { -489530532862885774L, 6511443832797L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_7_ = new BitSet(mk_tokenSet_7_());
	private static long[] mk_tokenSet_8_()
	{
		long[] data = { -4597025128503574416L, 6511443797837L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_8_ = new BitSet(mk_tokenSet_8_());
	private static long[] mk_tokenSet_9_()
	{
		long[] data = { -489486552397774222L, 6511443832799L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_9_ = new BitSet(mk_tokenSet_9_());
	private static long[] mk_tokenSet_10_()
	{
		long[] data = { 26743421523722368L, 2284922381312L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_10_ = new BitSet(mk_tokenSet_10_());
	private static long[] mk_tokenSet_11_()
	{
		long[] data = { 4107503391733710848L, 16910482L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_11_ = new BitSet(mk_tokenSet_11_());
	private static long[] mk_tokenSet_12_()
	{
		long[] data = { 79244294094848L, 131074L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_12_ = new BitSet(mk_tokenSet_12_());
	private static long[] mk_tokenSet_13_()
	{
		long[] data = { -489530532862885262L, 6511443832797L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_13_ = new BitSet(mk_tokenSet_13_());
	private static long[] mk_tokenSet_14_()
	{
		long[] data = { 4107494595640688640L, 690320L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_14_ = new BitSet(mk_tokenSet_14_());
	private static long[] mk_tokenSet_15_()
	{
		long[] data = { -4452901135744761744L, 6511443834703L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_15_ = new BitSet(mk_tokenSet_15_());
	private static long[] mk_tokenSet_16_()
	{
		long[] data = { 512L, 655360L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_16_ = new BitSet(mk_tokenSet_16_());
	private static long[] mk_tokenSet_17_()
	{
		long[] data = { -453492750772338062L, 6511443836927L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_17_ = new BitSet(mk_tokenSet_17_());
	private static long[] mk_tokenSet_18_()
	{
		long[] data = { 26742322012094592L, 2284922381312L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_18_ = new BitSet(mk_tokenSet_18_());
	private static long[] mk_tokenSet_19_()
	{
		long[] data = { 0L, 3145728L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_19_ = new BitSet(mk_tokenSet_19_());
	private static long[] mk_tokenSet_20_()
	{
		long[] data = { 0L, 8388608L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_20_ = new BitSet(mk_tokenSet_20_());
	private static long[] mk_tokenSet_21_()
	{
		long[] data = { 0L, 16777216L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_21_ = new BitSet(mk_tokenSet_21_());
	private static long[] mk_tokenSet_22_()
	{
		long[] data = { -345406548694007696L, 6511443832799L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_22_ = new BitSet(mk_tokenSet_22_());
	private static long[] mk_tokenSet_23_()
	{
		long[] data = { 256L, 13271040L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_23_ = new BitSet(mk_tokenSet_23_());
	private static long[] mk_tokenSet_24_()
	{
		long[] data = { 4107432943532638208L, 526482L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_24_ = new BitSet(mk_tokenSet_24_());
	private static long[] mk_tokenSet_25_()
	{
		long[] data = { 4121009793643053680L, 6511443807132L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_25_ = new BitSet(mk_tokenSet_25_());
	private static long[] mk_tokenSet_26_()
	{
		long[] data = { 4107494595640689152L, 690320L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_26_ = new BitSet(mk_tokenSet_26_());
	private static long[] mk_tokenSet_27_()
	{
		long[] data = { 8589935104L, 655360L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_27_ = new BitSet(mk_tokenSet_27_());
	private static long[] mk_tokenSet_28_()
	{
		long[] data = { 8804682957312L, 688130L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_28_ = new BitSet(mk_tokenSet_28_());
	private static long[] mk_tokenSet_29_()
	{
		long[] data = { 8796093022720L, 524290L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_29_ = new BitSet(mk_tokenSet_29_());
	private static long[] mk_tokenSet_30_()
	{
		long[] data = { 512L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_30_ = new BitSet(mk_tokenSet_30_());
	private static long[] mk_tokenSet_31_()
	{
		long[] data = { 4107432943532638720L, 526482L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_31_ = new BitSet(mk_tokenSet_31_());
	private static long[] mk_tokenSet_32_()
	{
		long[] data = { -468419909408259086L, 8796093018077L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_32_ = new BitSet(mk_tokenSet_32_());
	private static long[] mk_tokenSet_33_()
	{
		long[] data = { 13515198002364528L, 6511443772172L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_33_ = new BitSet(mk_tokenSet_33_());
	private static long[] mk_tokenSet_34_()
	{
		long[] data = { -453563198973410702L, 6511443804159L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_34_ = new BitSet(mk_tokenSet_34_());
	private static long[] mk_tokenSet_35_()
	{
		long[] data = { 13515198002364528L, 6511460582156L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_35_ = new BitSet(mk_tokenSet_35_());
	private static long[] mk_tokenSet_36_()
	{
		long[] data = { -4598170819619717008L, 6511443788557L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_36_ = new BitSet(mk_tokenSet_36_());
	private static long[] mk_tokenSet_37_()
	{
		long[] data = { 35753920387088624L, 8796067833612L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_37_ = new BitSet(mk_tokenSet_37_());
	private static long[] mk_tokenSet_38_()
	{
		long[] data = { 9011598374994032L, 6459635729164L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_38_ = new BitSet(mk_tokenSet_38_());
	private static long[] mk_tokenSet_39_()
	{
		long[] data = { 13515198002364656L, 8779189912332L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_39_ = new BitSet(mk_tokenSet_39_());
	private static long[] mk_tokenSet_40_()
	{
		long[] data = { -4597025128503573902L, 6511443797837L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_40_ = new BitSet(mk_tokenSet_40_());
	private static long[] mk_tokenSet_41_()
	{
		long[] data = { 0L, 9437184L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_41_ = new BitSet(mk_tokenSet_41_());
	private static long[] mk_tokenSet_42_()
	{
		long[] data = { 0L, 16809984L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_42_ = new BitSet(mk_tokenSet_42_());
	private static long[] mk_tokenSet_43_()
	{
		long[] data = { 1125899906842624L, 41984L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_43_ = new BitSet(mk_tokenSet_43_());
	private static long[] mk_tokenSet_44_()
	{
		long[] data = { 2251799813685248L, 1310720L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_44_ = new BitSet(mk_tokenSet_44_());
	private static long[] mk_tokenSet_45_()
	{
		long[] data = { 13515198002364528L, 6511460549388L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_45_ = new BitSet(mk_tokenSet_45_());
	private static long[] mk_tokenSet_46_()
	{
		long[] data = { 13515198002364528L, 6511452160780L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_46_ = new BitSet(mk_tokenSet_46_());
	private static long[] mk_tokenSet_47_()
	{
		long[] data = { -4597024956704882062L, 6511443797869L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_47_ = new BitSet(mk_tokenSet_47_());
	private static long[] mk_tokenSet_48_()
	{
		long[] data = { 35753920387088624L, 8796092999436L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_48_ = new BitSet(mk_tokenSet_48_());
	private static long[] mk_tokenSet_49_()
	{
		long[] data = { 2251799813685248L, 262144L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_49_ = new BitSet(mk_tokenSet_49_());
	private static long[] mk_tokenSet_50_()
	{
		long[] data = { 9011598374994032L, 6511443772172L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_50_ = new BitSet(mk_tokenSet_50_());
	private static long[] mk_tokenSet_51_()
	{
		long[] data = { 7599824371187712L, 67108864L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_51_ = new BitSet(mk_tokenSet_51_());
	private static long[] mk_tokenSet_52_()
	{
		long[] data = { 15766997816049776L, 6511444820748L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_52_ = new BitSet(mk_tokenSet_52_());
	private static long[] mk_tokenSet_53_()
	{
		long[] data = { 0L, 2199024304128L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_53_ = new BitSet(mk_tokenSet_53_());
	
}
}
