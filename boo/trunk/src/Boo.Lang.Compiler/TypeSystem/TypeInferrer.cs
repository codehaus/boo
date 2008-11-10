﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Boo.Lang.Compiler.TypeSystem
{
	class TypeInferrer : AbstractCompilerComponent 
	{
		Dictionary<IGenericParameter, InferredType> _inferredTypes = new Dictionary<IGenericParameter, InferredType>();

		public TypeInferrer()
		{
		}

		public TypeInferrer(IEnumerable<IGenericParameter> typeParameters)
		{
			InitializeTypeParameters(typeParameters);
		}

		public void InitializeTypeParameters(IEnumerable<IGenericParameter> typeParameters)
		{
			foreach (IGenericParameter typeParameter in typeParameters)
			{
				InferredTypes.Add(typeParameter, new InferredType());
			}
		}

		protected IDictionary<IGenericParameter, InferredType> InferredTypes
		{
			get { return _inferredTypes; }
		}

		/// <summary>
		/// Finalizes the inference by attempting to fix all inferred types.
		/// </summary>
		/// <returns>Whether the inference was completed successfully.</returns>
		public bool FinalizeInference()
		{
			foreach (InferredType inferredType in InferredTypes.Values)
			{
				if (!inferredType.Fixed) inferredType.Fix();
			}

			foreach (InferredType inferredType in InferredTypes.Values)
			{
				if (!inferredType.Fixed) return false;
			}	
			return true;
		}

		public IType GetInferredType(IGenericParameter gp)
		{
			if (InferredTypes.ContainsKey(gp))
			{
				return InferredTypes[gp].ResultingType;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Attempts to infer the type of generic parameters that occur in a formal parameter type
		/// according to its actual argument type. 
		/// </summary>
		/// <returns>False if inference failed; otherwise, true. </returns>
		public bool Infer(IType formalType, IType actualType)
		{
			return Infer(formalType, actualType, TypeInference.AllowCovariance);
		}

		/// <summary>
		/// Attempts to infer the type of generic parameters that occur in a formal parameter type
		/// according to its actual argument type. 
		/// </summary>
		/// <returns>False if inference failed; otherwise, true. </returns>
		protected bool Infer(IType formalType, IType actualType, TypeInference inference)
		{
			// Skip unspecified actual types
			if (actualType == null) return true;

			if (formalType is IGenericParameter)
			{
				return InferGenericParameter((IGenericParameter)formalType, actualType, inference);
			}

			if (formalType is ICallableType)
			{
				return InferCallableType((ICallableType)formalType, actualType, inference);
			}

			if (formalType.ConstructedInfo != null)
			{
				return InferConstructedType(formalType, actualType, inference);
			}

			if (formalType is IArrayType)
			{
				return InferArrayType((IArrayType)formalType, actualType, inference);
			}

			return InferSimpleType(formalType, actualType, inference);
		}

		private bool InferGenericParameter(IGenericParameter formalType, IType actualType, TypeInference inference)
		{
			if (InferredTypes.ContainsKey(formalType))
			{
				InferredType inferredType = InferredTypes[formalType];
				if ((inference & TypeInference.AllowContravariance) != TypeInference.AllowContravariance)
				{
					inferredType.ApplyLowerBound(actualType);
				}
				if ((inference & TypeInference.AllowCovariance) != TypeInference.AllowCovariance)
				{
					inferredType.ApplyUpperBound(actualType);
				}
			}

			return true;
		}

		private bool InferCallableType(ICallableType formalType, IType actualType, TypeInference inference)
		{
			ICallableType callableActualType = actualType as ICallableType;
			if (callableActualType == null) return false;

			CallableSignature formalSignature = formalType.GetSignature();
			CallableSignature actualSignature = callableActualType.GetSignature();

			// TODO: expand actual signature when it involves varargs?
			if (formalSignature.Parameters.Length != actualSignature.Parameters.Length) return false;

			// Infer return type, maintaining inference direction
			if (!Infer(formalSignature.ReturnType, actualSignature.ReturnType, inference))
			{
				return false;
			}

			// Infer parameter types, inverting inference direction
			for (int i = 0; i < formalSignature.Parameters.Length; ++i)
			{
				bool inferenceSuccessful = Infer(
					formalSignature.Parameters[i].Type,
					actualSignature.Parameters[i].Type,
					Invert(inference));

				if (!inferenceSuccessful) return false;
			}
			return true;
		}

		private bool InferConstructedType(IType formalType, IType actualType, TypeInference inference)
		{
			// look for a single occurance of the formal 
			// constructed type in the actual type's hierarchy 
			IType constructedActualType = GenericsServices.FindConstructedType(
				actualType,
				formalType.ConstructedInfo.GenericDefinition);

			if (constructedActualType == null)
			{
				return false;
			}

			// Exact inference requires the constructed occurance to be
			// the actual type itself
			if (inference == TypeInference.Exact && actualType != constructedActualType)
			{
				return false;
			}

			for (int i = 0; i < formalType.ConstructedInfo.GenericArguments.Length; ++i)
			{
				bool inferenceSuccessful = Infer(
					formalType.ConstructedInfo.GenericArguments[i],
					constructedActualType.ConstructedInfo.GenericArguments[i],
					TypeInference.Exact); // Generic arguments must match exactly, no variance allowed

				if (!inferenceSuccessful) return false;
			}
			return true;
		}

		private bool InferArrayType(IArrayType formalType, IType actualType, TypeInference inference)
		{
			IArrayType actualArrayType = actualType as IArrayType;
			return
				(actualArrayType != null) &&
				(actualArrayType.GetArrayRank() == formalType.GetArrayRank()) &&
				(Infer(formalType.GetElementType(), actualType.GetElementType(), inference));
		}

		private bool InferSimpleType(IType formalType, IType actualType, TypeInference inference)
		{
			// Inference has no effect on formal parameter types that are not generic parameters
			return true;
		}

		private TypeInference Invert(TypeInference inference)
		{
			switch (inference)
			{
				case TypeInference.AllowCovariance:
					return TypeInference.AllowContravariance;

				case TypeInference.AllowContravariance:
					return TypeInference.AllowCovariance;

				default:
					return TypeInference.Exact;
			}
		}
	}


	enum TypeInference
	{
		/// <summary>
		/// The type parameter must be set to the exact actual type.
		/// </summary>
		Exact = 0,

		/// <summary>
		/// The type parameter can be set to a supertype of the actual type.
		/// </summary>
		AllowCovariance = 1,

		/// <summary>
		/// The type parameter is allowed to be set to a type derived from the actual type.
		/// </summary>
		AllowContravariance = 2
	}
}
