using UnityEngine;
using UnityEngine.SceneManagement; // 씬 로드 감지를 위해 필수

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;       // BGM용 (Loop 체크 권장)
    public AudioSource sfxSource;       // 효과음용

    [Header("BGM Clips")]
    public AudioClip mainBGM;           // 타이틀 씬 음악
    public AudioClip gameBGM;           // 게임 플레이 씬 음악

    [Header("Player SFX")]
    public AudioClip attack;
    public AudioClip dash;
    public AudioClip defend;
    public AudioClip hurt;

    [Header("Player Skill SFX")]
    public AudioClip powerknockback;
    public AudioClip explosion;
    public AudioClip snow;

    [Header("Enemy SFX")]
    public AudioClip enemyHit;
    public AudioClip enemyDie;
    public AudioClip enemyKnockback;

    [Header("Boss Skill SFX")]
    public AudioClip shootFireBallWave;
    public AudioClip groundSmash;
    public AudioClip buff;

    [Header("Game SFX")]
    public AudioClip roundStart;
    public AudioClip roundClear;
    public AudioClip gameOver;
    public AudioClip typing;

    [Header("Button SFX")]
    public AudioClip buttonClick;

    private void Awake()
    {
        // 싱글톤 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 파괴되지 않음

            // 씬 로드 이벤트 구독 (씬이 바뀔 때마다 OnSceneLoaded 함수 실행)
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // 메모리 누수 방지를 위해 이벤트 구독 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 씬이 로드될 때마다 실행되는 콜백 함수
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 현재 로드된 씬의 이름에 따라 BGM 선택
        if (scene.name == "Title") // 유니티에 등록된 타이틀 씬 이름
        {
            PlayBGM(mainBGM);
        }
        else if (scene.name == "GameScene") // 유니티에 등록된 게임 씬 이름
        {
            PlayBGM(gameBGM);
        }
    }

    // -----------------------
    // BGM 관리
    // -----------------------
    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;

        // 이미 같은 음악이 재생 중이면 중복 실행하지 않음
        if (bgmSource.clip == clip && bgmSource.isPlaying) return;

        bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    // -----------------------
    // SFX 공통 재생 함수
    // -----------------------
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    // -----------------------
    // 개별 효과음 재생 함수들
    // -----------------------
    public void PlayAttack() { PlaySFX(attack); }
    public void PlayDash() { PlaySFX(dash); }
    public void PlayDefend() { PlaySFX(defend); }
    public void PlayHurt() { PlaySFX(hurt); }

    public void PlayPowerKnockback() { PlaySFX(powerknockback); }
    public void PlayExplosion() { PlaySFX(explosion); }
    public void PlaySnow() { PlaySFX(snow); }

    public void PlayShootFireBallWave() { PlaySFX(shootFireBallWave); }
    public void PlayGroundSmash() { PlaySFX(groundSmash); }
    public void PlayBuff() { PlaySFX(buff); }

    public void PlayEnemyHit() { PlaySFX(enemyHit); }
    public void PlayEnemyDie() { PlaySFX(enemyDie); }
    public void PlayEnemyKnockback() { PlaySFX(enemyKnockback); }

    public void PlayRoundStart() { PlaySFX(roundStart); }
    public void PlayRoundClear() { PlaySFX(roundClear); }
    public void PlayGameOver() { PlaySFX(gameOver); }
    public void PlayTyping() { PlaySFX(typing); }
    public void PlayClick() { PlaySFX(buttonClick); }
}