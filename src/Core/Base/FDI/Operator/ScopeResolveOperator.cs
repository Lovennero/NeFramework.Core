using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Framework.Core.FrameDI
{
    internal sealed class ScopeResolveOperator
    {
        private const string Tag =  "Scope Resolve Operator";
        
        private readonly FDIModel _model;
        
        // === 运行参数 ===
        private readonly List<int> _resolving;
        
        public ScopeResolveOperator(FDIModel model)
        {
            _model = model;
            
            _resolving = new List<int>();
        }

        // === 公开方法 ===
        public object Resolve(string scopeName, Type type)
        {
            var rtRecords = FindRegisterRecord(scopeName, type);
            
            // 类型关系存在检测
            if (rtRecords.Count == 0)
            {
                throw new InvalidOperationException($"[{Tag}]: Type:{type.FullName} does not exist.");
            }
            
            // 类型关系独立检测
            if (rtRecords.Count > 1)
            {
                throw new InvalidOperationException($"[{Tag}]: Type:{type.FullName} has more than one registration.");
            }
            
            // 拿到实施类
            var record = rtRecords[0];
            var implType = record.ImpType;
            
            // 查询缓存
            var itFactories = _model.InstanceFactories;
            if (!itFactories.TryGetValue(implType, out var factory))
            {
                // 构建工厂
                factory = RegisterFactoryBuild(implType);
                itFactories[implType] = factory;
            }
            
            // 执行解析
            var instance = RegisterResolve(scopeName, record, factory);
            
            // 执行注入
            Inject(scopeName, implType, instance);
            
            // 返回对象
            return instance;
        }
        public void Inject(string scopeName, Type implType, object instance)
        {
            // 查询缓存
            var ijFactories = _model.InjectFactories;
            if (!ijFactories.TryGetValue(implType, out var injectFactory))
            {
                // 构建工厂
                injectFactory = InjectFactoryBuild(implType);
                ijFactories[implType] = injectFactory;
            }
            
            // 执行注入
            injectFactory(this, scopeName, instance);
        }
        
        // === 内联方法 ===
        private IReadOnlyList<RegisterRecord> FindRegisterRecord(string scopeName, Type bindType)
        {
            var spRecords = _model.ScopeRecords;
            var rtRecords = _model.RegisterRecords;
            
            var currentScope = scopeName;
            while (currentScope != null)
            {
                if (!spRecords.TryGetValue(currentScope, out var spRecord) || !spRecord.Valid)
                {
                    throw new InvalidOperationException($"[{Tag}]: Scope {scopeName} does not exist.");
                }

                var spBtMap = spRecord.TypeBuildMap;
                if (spBtMap.TryGetValue(bindType, out var rtIDs))
                {
                    var list = new List<RegisterRecord>(rtIDs.Count);
                    foreach (var rtID in rtIDs)
                    {
                        var rtRecord = rtRecords[rtID];
                        list.Add(rtRecord);
                    }
                    return list;
                }

                currentScope = spRecord.Parent;
            }

            return Array.Empty<RegisterRecord>();
        }

        // === 构建实例工厂 ===
        private InstanceFactory RegisterFactoryBuild(Type implType)
        {
            // 拿到构造函数和参数
            var ctor = LongestConstructorGet(implType);
            var parameters = ctor.GetParameters();
            
            // 创建参数表达式
            var opParam = Expression.Parameter(typeof(ScopeResolveOperator), "spResolveOp");
            var scopeParam = Expression.Parameter(typeof(string), "scopeName");
            
            // 反射拿到解析方法
            var resolveMethod = typeof(ScopeResolveOperator).GetMethod(nameof(ScopeResolveOperator.Resolve), BindingFlags.Public | BindingFlags.Instance);
            if (resolveMethod == null)
            {
                throw new InvalidOperationException($"[{Tag}]: Operator resolve Method does not exist.");
            }
            
            // 构建参数表达式
            var args = new Expression[parameters.Length];
            for (var i =  0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                var parameterType = parameter.ParameterType;

                // 可选参数不参与解析，直接使用默认值
                if (parameter.HasDefaultValue)
                {
                    args[i] = DefaultValueExpressionBuild(parameter, parameterType);
                    continue;
                }

                var call = Expression.Call(opParam, resolveMethod, scopeParam, Expression.Constant(parameterType, typeof(Type)));
                args[i] = Expression.Convert(call, parameterType);
            }
            Expression body = Expression.Convert(Expression.New(ctor, args), typeof(object));
            var factory = Expression.Lambda<InstanceFactory>(body, opParam, scopeParam).Compile();
            
            return factory;
        }
        private ConstructorInfo LongestConstructorGet(Type type)
        {
            // 检测传入类型的合法性
            if (type.IsAbstract || type.IsInterface)
            {
                throw new InvalidOperationException($"[{Tag}]: Type:{type} does not implement Type.");
            }

            // 获取所有构造函数信息
            var ctInfos = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            if (ctInfos.Length == 0)
            {
                throw new InvalidOperationException($"[{Tag}]: Type:{type.Name} has not found matching constructor.(Public|Instance)");
            }
            
            // 筛选参数最多构造函数
            var maxLen = int.MinValue;
            ConstructorInfo info = null;
            var ambiguous = false;
            foreach (var ctInfo in ctInfos)
            {
                var paramCount = ctInfo.GetParameters().Length;
                if (paramCount > maxLen)
                {
                    info = ctInfo;
                    maxLen = paramCount;
                    ambiguous = false;
                    continue;
                }
                
                if (paramCount == maxLen)
                {
                    ambiguous = true;
                }
            }

            // 构造函数参数相同检测
            if (ambiguous)
            {
                throw new InvalidOperationException($"[{Tag}]: Type:{type.Name} has ambiguous constructor.");
            }
            
            return info;
        }
        private Expression DefaultValueExpressionBuild(ParameterInfo parameter, Type parameterType)
        {
            var defaultValue = parameter.DefaultValue;

            // 部分编译器对可选参数不写入默认值，回退到类型默认值
            if (defaultValue == null || defaultValue == DBNull.Value || defaultValue == Type.Missing)
            {
                return Expression.Default(parameterType);
            }

            // 枚举默认值以底层整型存储，需要转回枚举类型
            if (parameterType.IsEnum)
            {
                return Expression.Constant(Enum.ToObject(parameterType, defaultValue), parameterType);
            }

            return Expression.Constant(defaultValue, parameterType);
        }

        // === 构建注入工厂 ===
        private InjectFactory InjectFactoryBuild(Type implType)
        {
            // 注：工厂目标：1.转换类型 2.解析依赖 3.属性赋值
            var props = InjectPropertiesGet(implType);
            
            var opParam = Expression.Parameter(typeof(ScopeResolveOperator), "spResolveOp");
            var scopeParam = Expression.Parameter(typeof(string), "scopeName");
            var objectParam = Expression.Parameter(typeof(object), "instance");
            
            var executeMethod = typeof(ScopeResolveOperator).GetMethod(nameof(Resolve), BindingFlags.Public | BindingFlags.Instance);
            if (executeMethod == null)
            {
                throw new InvalidOperationException($"[{Tag}]: Operator resolve Method does not exist.");
            }
            
            var instanceExp = Expression.Convert(objectParam, implType);

            var expressions = new List<Expression>();
            for (var i = 0; i < props.Count; i++)
            {
                var prop = props[i];
                
                var propMemberExp = Expression.Property(instanceExp, prop);
                var call = Expression.Call(opParam, executeMethod, scopeParam, Expression.Constant(prop.PropertyType, typeof(Type)));
                var expression = Expression.Assign(propMemberExp, Expression.Convert(call, prop.PropertyType));
                expressions.Add(expression);
            }
            
            var block = Expression.Block(expressions);
            var lambda = Expression.Lambda<InjectFactory>(block, opParam, scopeParam, objectParam);
            return lambda.Compile();
        }
        private IReadOnlyList<PropertyInfo> InjectPropertiesGet(Type type)
        {
            // 检测传入类型的合法性
            if (type.IsAbstract || type.IsInterface)
            {
                throw new InvalidOperationException($"[{Tag}]: Type:{type} does not implement Type.");
            }
            
            var members = new List<PropertyInfo>();
            
            // 获取所有属性信息
            var currentType = type;
            while (currentType != null && currentType != typeof(object))
            {
                var props  = currentType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Public |  BindingFlags.DeclaredOnly);
                foreach (var prop in props )
                {
                    if (prop.GetCustomAttribute<InjectAttribute>() == null) continue;
                    if (!prop.CanWrite)
                    {
                        throw new InvalidOperationException($"[{Tag}]: Property {prop.Name} is not writable.");
                    }
                    members.Add(prop);
                }
                
                currentType = currentType.BaseType;
            }
            
            return members;
        }
        
        // === 创建实例 ===
        private object RegisterResolve(string scopeName, RegisterRecord record, InstanceFactory factory)
        {
            var spRecords = _model.ScopeRecords;
            var itObjects = _model.TypeInstances;

            if (record.Lifetime == ELifetime.Singleton)
            {
                var crScope = record.Name;
                if (!spRecords.TryGetValue(crScope, out var spRecord))
                {
                    throw new InvalidOperationException($"[{Tag}]: Scope {scopeName} does not exist.");
                }
                
                var itMap = spRecord.RegisterInstanceMap;
                if (!itMap.TryGetValue(record.SerialID, out var itID))
                {
                    // 创建实例
                    var instance = CreateInstance(crScope, record, factory);
                    
                    // 添加到全局缓存
                    var instanceID = _model.InstanceID++;
                    itObjects.Add(instanceID, instance);
                    
                    // 注册到作用域缓存
                    itID = instanceID;
                    itMap[record.SerialID] = itID;
                }
                
                return itObjects[itID];
            }
            
            if (record.Lifetime == ELifetime.Scoped)
            {
                var crScope = scopeName;
                if (!spRecords.TryGetValue(crScope, out var spRecord))
                {
                    throw new InvalidOperationException($"[{Tag}]: Scope {scopeName} does not exist.");
                }
                
                var itMap = spRecord.RegisterInstanceMap;
                if (!itMap.TryGetValue(record.SerialID, out var itID))
                {
                    // 创建实例
                    var instance = CreateInstance(crScope, record, factory);
                    
                    // 添加到全局缓存
                    var instanceID = _model.InstanceID++;
                    itObjects.Add(instanceID, instance);
                    
                    // 注册到作用域缓存
                    itID = instanceID;
                    itMap[record.SerialID] = itID;
                }
                
                return itObjects[itID];
            }

            if (record.Lifetime == ELifetime.Transient)
            {
                var crScope = scopeName;

                // 创建实例
                var instance = CreateInstance(crScope, record, factory);
                return instance;
            }
            
            throw new InvalidOperationException($"[{Tag}]: Record:{record.ImpType.FullName} lifetime is unrecognized.");
        }
        private object CreateInstance(string scopeName, RegisterRecord record, InstanceFactory factory)
        {
            var rtRecords = _model.RegisterRecords;
            
            if (_resolving.Contains(record.SerialID))
            {
                var sb = new StringBuilder($"[{Tag}]: Circular dependency: ");
                foreach (var id in _resolving)
                {
                    sb.Append(rtRecords[id].ImpType.Name).Append(" → ");
                }
                sb.Append(record.ImpType.Name);
                
                throw new InvalidOperationException(sb.ToString());
            }

            if (_resolving.Count >= 256)
            {
                throw new InvalidOperationException($"[{Tag}]: Resolve depth exceeded {256}.");
            }
            
            _resolving.Add(record.SerialID);

            
            try
            {
                return factory(this, scopeName);
            }
            finally
            {
                _resolving.RemoveAt(_resolving.Count - 1);
            }
        }
    }
}