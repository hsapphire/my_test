/**
●
创建了一个新的 Solana Keypair (counterAccountKp) 用于存储计数器的状态。
●
使用 Solana API 获取在链上创建相应账户所需的最小 lamports，即Solana 链上存储该账户所要支付的最小押金rent。
●
构建createGreetingAccountIx指令，在链上创建我们指定的counterAccountKp.publicKey账户，并指定了账户的大小。
 */
// 创建 keypair
const counterAccountKp = new web3.Keypair();
console.log(`counterAccountKp.publickey : ${counterAccountKp.publicKey}`)
const lamports = await pg.connection.getMinimumBalanceForRentExemption(
	GREETING_SIZE
);

// 创建生成对应数据账户的指令
const createGreetingAccountIx = web3.SystemProgram.createAccount({
	fromPubkey: pg.wallet.publicKey,
	lamports,
	newAccountPubkey: counterAccountKp.publicKey,
	programId: pg.PROGRAM_ID,
	space: GREETING_SIZE,
});


