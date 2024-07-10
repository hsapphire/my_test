// No imports needed: web3, borsh, pg and more are globally available
import { PublicKey } from '@solana/web3.js';

/**
 * CounterAccount 对象
 */
class CounterAccount {
  count = 0;
  constructor(fields: { count: number } | undefined = undefined) {
    if (fields) {
      this.count = fields.count;
    }
  }
}

/**
 * CounterAccount 对象 schema 定义
 */
const CounterAccountSchema = new Map([
  [CounterAccount, { kind: "struct", fields: [["count", "u32"]] }],
]);



describe("Test", () => {
  it("greet", async () => {

    const counterAccountPk = new PublicKey("3EsU9Tg8V186EEaPvfdYmrof996soBNBk8ivbsAxsM6w")
    // 调用程序,计数器累加
    const greetIx = new web3.TransactionInstruction({
      keys: [
        {
          pubkey: counterAccountPk,
          isSigner: false,
          isWritable: true,
        },
      ],
      programId: pg.PROGRAM_ID,
    });

    // 创建交易
    const tx = new web3.Transaction();
    tx.add(greetIx);

    // 发起交易，获取交易哈希
    const txHash = await web3.sendAndConfirmTransaction(pg.connection, tx, [
      pg.wallet.keypair,
    ]);
    console.log(`Use 'solana confirm -v ${txHash}' to see the logs`);

    // 获取指定数据账户的信息
    const counterAccountOnSolana = await pg.connection.getAccountInfo(
      counterAccountPk
    );

    // 反序列化
    const deserializedAccountData = borsh.deserialize(
      CounterAccountSchema,
      CounterAccount,
      counterAccountOnSolana.data
    );

    // 判断当前计数器是否累加
    assert.equal(deserializedAccountData.count, 4);
  });
});