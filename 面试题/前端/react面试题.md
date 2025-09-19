## 一、React 基础类面试题

### 1. React 中的虚拟 DOM 是什么？它是如何工作的？

**答案：**
虚拟 DOM（Virtual DOM）是 React 通过 JavaScript 对真实 DOM 的一种抽象表示。当状态发生变化时，React 会先在虚拟 DOM 中构建新的 UI 树，然后与上一次的虚拟 DOM 树进行对比（Diffing），找出最小变更集合，最后更新真实 DOM，提升性能。

---

### 2. React 中的组件有哪几种？区别是什么？

**答案：**
- dd 
- dd 
  - d 
  - d 
  - d
  - 
* 函数组件（Function Component）：无状态组件，使用 `Hooks` 后可以管理状态。
* 类组件（Class Component）：传统组件，**通过 **`this.state` 和生命周期方法管理状态。

区别：

* 函数组件语法更简洁；
* 使用 `Hooks` 后功能上与类组件无太大差别；
* 类组件已被官方推荐逐步弃用。

---

### 3. 组件的生命周期方法有哪些？

**答案：**（以类组件为例）

* 挂载阶段：`constructor` → `componentDidMount`
* 更新阶段：`shouldComponentUpdate` → `componentDidUpdate`
* 卸载阶段：`componentWillUnmount`

函数组件使用 `useEffect` 模拟生命周期行为。

---

## 🔄 二、Hooks 相关面试题

### 4. `useEffect` 的用途？与 `componentDidMount` 有什么区别？

**答案：**

* `useEffect` 可以模拟 `componentDidMount`、`componentDidUpdate` 和 `componentWillUnmount`。
* 通过第二个参数控制执行时机：
  * `[]`：仅初始化执行一次（componentDidMount）
  * `[deps]`：依赖变更时执行（componentDidUpdate）
  * 返回清理函数时可模拟 `componentWillUnmount`

---

### 5. `useCallback` 和 `useMemo` 的区别？

**答案：**

* `useMemo(fn, deps)`：缓存函数的**返回值**
* `useCallback(fn, deps)`：缓存**函数本身**

避免组件重复渲染或重复创建函数对象。

---

### 6. 如何自定义 Hook？举例说明。

**答案：**

<pre class="overflow-visible!" data-start="1185" data-end="1543"><div class="contain-inline-size rounded-md border-[0.5px] border-token-border-medium relative bg-token-sidebar-surface-primary"><div class="flex items-center text-token-text-secondary px-4 py-2 text-xs font-sans justify-between h-9 bg-token-sidebar-surface-primary dark:bg-token-main-surface-secondary select-none rounded-t-[5px]">js</div><div class="sticky top-9"><div class="absolute end-0 bottom-0 flex h-9 items-center pe-2"><div class="bg-token-sidebar-surface-primary text-token-text-secondary dark:bg-token-main-surface-secondary flex items-center rounded-sm px-2 font-sans text-xs"><button class="flex gap-1 items-center select-none px-4 py-1" aria-label="Copy"><svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg" class="icon-xs"><path fill-rule="evenodd" clip-rule="evenodd" d="M7 5C7 3.34315 8.34315 2 10 2H19C20.6569 2 22 3.34315 22 5V14C22 15.6569 20.6569 17 19 17H17V19C17 20.6569 15.6569 22 14 22H5C3.34315 22 2 20.6569 2 19V10C2 8.34315 3.34315 7 5 7H7V5ZM9 7H14C15.6569 7 17 8.34315 17 10V15H19C19.5523 15 20 14.5523 20 14V5C20 4.44772 19.5523 4 19 4H10C9.44772 4 9 4.44772 9 5V7ZM5 9C4.44772 9 4 9.44772 4 10V19C4 19.5523 4.44772 20 5 20H14C14.5523 20 15 19.5523 15 19V10C15 9.44772 14.5523 9 14 9H5Z" fill="currentColor"></path></svg>Copy</button><span class="" data-state="closed"><button class="flex items-center gap-1 px-4 py-1 select-none"><svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg" class="icon-xs"><path d="M2.5 5.5C4.3 5.2 5.2 4 5.5 2.5C5.8 4 6.7 5.2 8.5 5.5C6.7 5.8 5.8 7 5.5 8.5C5.2 7 4.3 5.8 2.5 5.5Z" fill="currentColor" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round"></path><path d="M5.66282 16.5231L5.18413 19.3952C5.12203 19.7678 5.09098 19.9541 5.14876 20.0888C5.19933 20.2067 5.29328 20.3007 5.41118 20.3512C5.54589 20.409 5.73218 20.378 6.10476 20.3159L8.97693 19.8372C9.72813 19.712 10.1037 19.6494 10.4542 19.521C10.7652 19.407 11.0608 19.2549 11.3343 19.068C11.6425 18.8575 11.9118 18.5882 12.4503 18.0497L20 10.5C21.3807 9.11929 21.3807 6.88071 20 5.5C18.6193 4.11929 16.3807 4.11929 15 5.5L7.45026 13.0497C6.91175 13.5882 6.6425 13.8575 6.43197 14.1657C6.24513 14.4392 6.09299 14.7348 5.97903 15.0458C5.85062 15.3963 5.78802 15.7719 5.66282 16.5231Z" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path><path d="M14.5 7L18.5 11" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path></svg>Edit</button></span></div></div></div><div class="overflow-y-auto p-4" dir="ltr"><code class="whitespace-pre! language-js"><span><span>import</span><span> { useState, useEffect } </span><span>from</span><span> </span><span>'react'</span><span>;

</span><span>function</span><span> </span><span>useWindowWidth</span><span>(</span><span></span><span>) {
  </span><span>const</span><span> [width, setWidth] = </span><span>useState</span><span>(</span><span>window</span><span>.</span><span>innerWidth</span><span>);
  </span><span>useEffect</span><span>(</span><span>() =></span><span> {
    </span><span>const</span><span> </span><span>onResize</span><span> = (</span><span></span><span>) => </span><span>setWidth</span><span>(</span><span>window</span><span>.</span><span>innerWidth</span><span>);
    </span><span>window</span><span>.</span><span>addEventListener</span><span>(</span><span>'resize'</span><span>, onResize);
    </span><span>return</span><span> </span><span>() =></span><span> </span><span>window</span><span>.</span><span>removeEventListener</span><span>(</span><span>'resize'</span><span>, onResize);
  }, []);
  </span><span>return</span><span> width;
}
</span></span></code></div></div></pre>

---

## 📦 三、工程化与性能优化

### 7. 如何优化 React 应用的性能？

**答案：**

* 使用 `React.memo`、`useMemo`、`useCallback` 避免不必要渲染；
* 懒加载组件（`React.lazy` + `Suspense`）；
* 虚拟列表（如 `react-window`）；
* 拆分大组件，提升复用性；
* 使用代码分割（Webpack + 动态导入）；
* 合理使用 `key` 避免 diff 错误。

---

### 8. React 项目中状态管理的解决方案有哪些？

**答案：**

* 原生：`useState`, `useReducer`, `Context API`
* 外部库：Redux, MobX, Recoil, Zustand, Jotai 等
* 推荐中小项目使用 `useContext` + `useReducer`，大型项目考虑 Redux Toolkit

---

### 9. 如何处理 React 中的跨组件通信？

**答案：**

* 父子通信：props
* 兄弟通信：提升状态、Context
* 远距离通信：Redux、EventBus、Context、URL Params

---

## 🔧 四、实际开发与项目实践

### 10. 项目中如何处理表单和验证？

**答案：**

* 原生表单控件配合 `useState`
* 使用库：Formik + Yup、React Hook Form，结合 yup/zod 做 schema 验证

---

### 11. 如何在 React 中处理路由？

**答案：**

* 使用 `react-router-dom`
  <pre class="overflow-visible!" data-start="2289" data-end="2413"><div class="contain-inline-size rounded-md border-[0.5px] border-token-border-medium relative bg-token-sidebar-surface-primary"><div class="flex items-center text-token-text-secondary px-4 py-2 text-xs font-sans justify-between h-9 bg-token-sidebar-surface-primary dark:bg-token-main-surface-secondary select-none rounded-t-[5px]">js</div><div class="sticky top-9"><div class="absolute end-0 bottom-0 flex h-9 items-center pe-2"><div class="bg-token-sidebar-surface-primary text-token-text-secondary dark:bg-token-main-surface-secondary flex items-center rounded-sm px-2 font-sans text-xs"><button class="flex gap-1 items-center select-none px-4 py-1" aria-label="Copy"><svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg" class="icon-xs"><path fill-rule="evenodd" clip-rule="evenodd" d="M7 5C7 3.34315 8.34315 2 10 2H19C20.6569 2 22 3.34315 22 5V14C22 15.6569 20.6569 17 19 17H17V19C17 20.6569 15.6569 22 14 22H5C3.34315 22 2 20.6569 2 19V10C2 8.34315 3.34315 7 5 7H7V5ZM9 7H14C15.6569 7 17 8.34315 17 10V15H19C19.5523 15 20 14.5523 20 14V5C20 4.44772 19.5523 4 19 4H10C9.44772 4 9 4.44772 9 5V7ZM5 9C4.44772 9 4 9.44772 4 10V19C4 19.5523 4.44772 20 5 20H14C14.5523 20 15 19.5523 15 19V10C15 9.44772 14.5523 9 14 9H5Z" fill="currentColor"></path></svg>Copy</button><span class="" data-state="closed"><button class="flex items-center gap-1 px-4 py-1 select-none"><svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg" class="icon-xs"><path d="M2.5 5.5C4.3 5.2 5.2 4 5.5 2.5C5.8 4 6.7 5.2 8.5 5.5C6.7 5.8 5.8 7 5.5 8.5C5.2 7 4.3 5.8 2.5 5.5Z" fill="currentColor" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round"></path><path d="M5.66282 16.5231L5.18413 19.3952C5.12203 19.7678 5.09098 19.9541 5.14876 20.0888C5.19933 20.2067 5.29328 20.3007 5.41118 20.3512C5.54589 20.409 5.73218 20.378 6.10476 20.3159L8.97693 19.8372C9.72813 19.712 10.1037 19.6494 10.4542 19.521C10.7652 19.407 11.0608 19.2549 11.3343 19.068C11.6425 18.8575 11.9118 18.5882 12.4503 18.0497L20 10.5C21.3807 9.11929 21.3807 6.88071 20 5.5C18.6193 4.11929 16.3807 4.11929 15 5.5L7.45026 13.0497C6.91175 13.5882 6.6425 13.8575 6.43197 14.1657C6.24513 14.4392 6.09299 14.7348 5.97903 15.0458C5.85062 15.3963 5.78802 15.7719 5.66282 16.5231Z" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path><path d="M14.5 7L18.5 11" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path></svg>Edit</button></span></div></div></div><div class="overflow-y-auto p-4" dir="ltr"><code class="whitespace-pre! language-js"><span><span><</span><span>Routes</span><span>>
    </span><span><span class="language-xml"><Route</span></span><span> </span><span>path</span><span>=</span><span>"/"</span><span> </span><span>element</span><span>=</span><span>{</span><span><</span><span>Home</span><span> />} />
    </span><span><span class="language-xml"><Route</span></span><span> </span><span>path</span><span>=</span><span>"/about"</span><span> </span><span>element</span><span>=</span><span>{</span><span><</span><span>About</span><span> />} />
  </</span><span>Routes</span><span>>
  </span></span></code></div></div></pre>

---

### 12. 前端如何处理 SSR 和 SEO？

**答案：**

* 使用 Next.js 实现 React SSR；
* 提供 `getServerSideProps`, `getStaticProps` 支持数据预渲染；
* 利用 `Head` 设置标题和 meta 信息；
* 针对 SPA 的 SEO，需配合服务器渲染或使用 Prerendering。

---

### 13. 跨域问题如何处理？

**答案：**

* 设置后端允许跨域（`Access-Control-Allow-Origin`）
* 前端开发阶段使用代理（如 Vite、Webpack devServer 配置 `proxy`）

---

## 🧪 五、加分项/源码层面（适合中高级）

### 14. React Fiber 是什么？

**答案：**
React Fiber 是 React 16 引入的协调引擎，优化了渲染性能。它将渲染工作拆分为可中断的单元，使得 React 可以更平滑地更新 UI，并为并发模式（Concurrent Mode）打下基础。

---

### 15. React 如何实现 diff 算法？

**答案：**
React 使用**双端比较**和**同层对比**策略来优化性能：

* 不同类型直接替换；
* 相同类型按 key 和顺序比较；
* 避免深层递归，降低复杂度为 O(n)

---

如需题库 PDF、模拟面试流程或算法题集等，我可以为你定制更多内容。是否需要我生成一份**可导出的题库文件（PDF/Excel）**？
