using UnityEngine;
using System.Collections.Generic;
using System.Linq;

#region Head
/*
 * [OverView]	サウンド管理クラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
public class ProjectSoundManager : Singleton<ProjectSoundManager> {

	#region class
	// BGM情報
	private class BGMData {

		#region variable
		public Dictionary<BGMEnum.ID, AudioClip> dictionary = new Dictionary<BGMEnum.ID, AudioClip>();
		public AudioSource source = null;
		public BGMEnum.ID nextID = BGMEnum.ID.NONE;
		public float fadeRate = 0.0f;
		public bool isChange = false;
		#endregion
	}

	// SE情報
	private class SEData {

		#region class
		public class DelayPlay {

			#region variable
			public SEEnum.ID seID = SEEnum.ID.NONE;
			public float delay = 0.0f;
			#endregion
		}
		#endregion

		#region variable
		public Dictionary<SEEnum.ID, AudioClip> dictionary = new Dictionary<SEEnum.ID, AudioClip>();
		public List<AudioSource> sourceList = new List<AudioSource>();
		public List<DelayPlay> delayList = new List<DelayPlay>();
		#endregion
	}
	#endregion

	#region const_variable
	private const float _defaultBGMFadeRate = 0.5f;	// BGMの基本フェード
	private const int _maxSEPlay = 10;				// SEの同時実行可能数
	#endregion

	#region variable
	private BGMData _bgmData = new BGMData();		// BGM情報
	private SEData _seData = new SEData();			// SE情報
	private SoundData _soundData = null;			// サウンド情報
	#endregion

	#region function
	// 初期化
	protected override void Initialize()
	{
		// サウンドデータ取得
		_soundData = Resources.Load<SoundData>(FileName.SoundDataFile);

		// SE初期化
		StartupSE();

		// BGM初期化
		StartupBGM();
	}

	// 更新
	private void Update()
	{
		// BGM更新
		RunBGM();

		// SE更新
		RunSE();
	}

	// 設定番号取得
	private bool ConvertSoundNumber(string name, out int number)
	{
		// 番号初期化
		number = 0;

		// 選択サウンド情報取得
		SoundData.Param soundParam = _soundData.list.FirstOrDefault(sound => sound.Name == name);

		// 取得に失敗したら
		if (soundParam == null)
			return false;

		// 番号設定
		number = soundParam.No;
		return true;
	} 

	// 設定音量取得
	private bool ConvertSoundVolume(int number, out float volume)
	{
		// 音量初期化
		volume = 0.0f;

		// 選択サウンド情報取得
		SoundData.Param soundParam = _soundData.list.FirstOrDefault(sound => sound.No == number);

		// 取得に失敗したら
		if (soundParam == null)
			return false;

		// 音量設定
		volume = soundParam.Volume;
		return true;
	}

	// BGM初期化
	private void StartupBGM()
	{

	}

	// BGM更新
	private void RunBGM()
	{
		// 変更がなければ処理しない
		if (!_bgmData.isChange)
			return;
		
		// ボリュームを下げる
		_bgmData.source.volume -= Time.deltaTime * _bgmData.fadeRate;

		// 消音になったら
		if (_bgmData.source.volume <= 0.0f) {

			// BGM停止
			_bgmData.source.Stop();

			// 変更完了
			_bgmData.isChange = false;

			// BGM再生開始
			PlayStartBGM(_bgmData.nextID);
		}
	}

	// BGM再生
	public void PlayBGM(BGMEnum.ID bgmID, float fadeRate = _defaultBGMFadeRate)
	{
		// BGMが使用可能か確認
		if (!_bgmData.dictionary.ContainsKey(bgmID))
			return;

		// 変更レート最適化
		fadeRate = Mathf.Max(0.0f, fadeRate);

		// 再生中なら
		if (_bgmData.source.isPlaying) {

			// 次のBGMとして登録
			_bgmData.nextID = bgmID;

			// 変更設定
			_bgmData.fadeRate = fadeRate;
			_bgmData.isChange = true;
			return;
		}

		// 再生開始
		PlayStartBGM(bgmID);
	}

	// BGM停止
	public void StopBGM()
	{
		// その場で停止
		_bgmData.source.Stop();
	}

	// BGM再生開始
	private void PlayStartBGM(BGMEnum.ID bgmID)
	{
		// BGM音量取得
		float volume = 0.0f;
		if (ConvertSoundVolume((int)bgmID, out volume))
			return;

		// BGM音量設定
		_bgmData.source.volume = volume;

		// BGM設定
		_bgmData.source.clip = _bgmData.dictionary[bgmID];

		// BGM再生
		_bgmData.source.Play();
	}

	// SE初期化
	private void StartupSE()
	{
		// SEファイル読込
		object[] seResource = Resources.LoadAll(DirectoryPath.SEPath);

		// SE情報取得

	}

	// SE更新
	private void RunSE()
	{
		// 待機中のSEがなければ処理しない
		if (_seData.delayList.Count == 0)
			return;
		
	}

	// SE再生
	public void PlaySE(SEEnum.ID seID, float delay = 0.0f)
	{
		// SEが使用可能か確認
		if (!_seData.dictionary.ContainsKey(seID))
			return;

		// 遅延時間最適化
		delay = Mathf.Max(0.0f, delay);

		// 遅延があれば
		if(delay > 0.0f) {

			// データ登録
			SEData.DelayPlay delayPlay = new SEData.DelayPlay();
			delayPlay.seID = seID;
			delayPlay.delay = delay;
			_seData.delayList.Add(delayPlay);
			return;
		}

		// SE再生開始
		PlayStartSE(seID);
	}

	// SE停止
	public void StopSE()
	{
		// 全情報確認する
		foreach (AudioSource source in _seData.sourceList) {

			// 再生中でなければ次へ
			if (!source.isPlaying)
				continue;

			// その場でSE停止
			source.Stop();
		}
	}

	// SE再生開始
	private void PlayStartSE(SEEnum.ID seID)
	{
		// 空きがあれば再生
		foreach(AudioSource source in _seData.sourceList) {

			// 未再生であれば
			if(!source.isPlaying) {

				// SE音量取得
				float volume = 0.0f;
				if (ConvertSoundVolume((int)seID, out volume))
					return;

				// SE音量設定
				source.volume = volume;

				// SE再生
				source.PlayOneShot(_seData.dictionary[seID]);
				break;
			}
		}
	}
	#endregion
}