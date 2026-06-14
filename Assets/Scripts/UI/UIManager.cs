using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private Text killCounterText;

    private int killCount;
    private bool _subscribed;

    private GameObject _healthContainer;
    private Image[] _healthSegments;
    private int _maxHealth;
    private Text _heartIcon;

    private void Awake()
    {
        Instance = this;
        if (killCounterText == null)
            SetupUI();
    }

    private void SetupUI()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas = canvasGO.GetComponent<Canvas>();
            canvasGO.transform.SetParent(transform, false);
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvasGO.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
        }

        SetupHealthBar(canvas);
        SetupKillCounter(canvas);
    }

    private void SetupHealthBar(Canvas canvas)
    {
        _maxHealth = Player.Instantce != null ? Player.Instantce.GetMaxHealth() : 10;

        _healthContainer = new GameObject("HealthBarContainer", typeof(RectTransform));
        _healthContainer.transform.SetParent(canvas.transform, false);

        RectTransform containerRect = _healthContainer.GetComponent<RectTransform>();
        containerRect.anchorMin = new Vector2(0, 1);
        containerRect.anchorMax = new Vector2(0, 1);
        containerRect.pivot = new Vector2(0, 1);
        containerRect.sizeDelta = new Vector2(_maxHealth * 30 + 50, 36);
        containerRect.anchoredPosition = new Vector2(20, -20);

        _healthSegments = new Image[_maxHealth];

        for (int i = 0; i < _maxHealth; i++)
        {
            GameObject segment = new GameObject($"Segment_{i}", typeof(Image));
            segment.transform.SetParent(_healthContainer.transform, false);

            RectTransform segRect = segment.GetComponent<RectTransform>();
            segRect.sizeDelta = new Vector2(24, 28);
            segRect.anchoredPosition = new Vector2(2 + i * 28, 0);

            Image segImage = segment.GetComponent<Image>();
            segImage.color = new Color(0.2f, 0.2f, 0.2f, 0.5f);

            _healthSegments[i] = segImage;
        }

        GameObject heartGO = new GameObject("HeartIcon", typeof(Text));
        heartGO.transform.SetParent(_healthContainer.transform, false);

        RectTransform heartRect = heartGO.GetComponent<RectTransform>();
        heartRect.sizeDelta = new Vector2(36, 36);
        heartRect.anchoredPosition = new Vector2(_maxHealth * 28 + 8, 2);

        _heartIcon = heartGO.GetComponent<Text>();
        _heartIcon.text = "\u2665";
        _heartIcon.fontSize = 32;
        _heartIcon.alignment = TextAnchor.MiddleCenter;
        _heartIcon.color = new Color(1, 0.2f, 0.2f);
        _heartIcon.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        if (Player.Instantce != null)
            RefreshHealthSegments(Player.Instantce.GetCurrentHealth());
    }

    private void SetupKillCounter(Canvas canvas)
    {
        GameObject textGO = new GameObject("KillCounter", typeof(Text));
        textGO.transform.SetParent(canvas.transform, false);

        RectTransform textRect = textGO.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(1, 1);
        textRect.anchorMax = new Vector2(1, 1);
        textRect.pivot = new Vector2(1, 1);
        textRect.sizeDelta = new Vector2(200, 50);
        textRect.anchoredPosition = new Vector2(-20, -20);

        killCounterText = textGO.GetComponent<Text>();
        killCounterText.text = "Kills: 0";
        killCounterText.fontSize = 28;
        killCounterText.alignment = TextAnchor.MiddleRight;
        killCounterText.color = Color.white;
        killCounterText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
    }

    private void RefreshHealthSegments(int currentHealth)
    {
        for (int i = 0; i < _healthSegments.Length; i++)
        {
            if (i < currentHealth)
            {
                float t = (float)i / (_maxHealth - 1);
                Color activeColor = Color.Lerp(new Color(0.2f, 1, 0.2f), new Color(1, 0.2f, 0.2f), t);
                _healthSegments[i].color = activeColor;
            }
            else
            {
                _healthSegments[i].color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
            }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SubscribeToPlayer();
    }

    private void SubscribeToPlayer()
    {
        if (_subscribed)
        {
            if (Player.Instantce != null)
            {
                Player.Instantce.OnPlayerDamaged -= UpdateHealthBar;
                Player.Instantce.OnPlayerDeath -= OnPlayerDeath;
            }
            EnemyEntity.OnAnyEnemyDeath -= OnEnemyDeath;
            _subscribed = false;
        }

        if (Player.Instantce != null)
        {
            int maxHealth = Player.Instantce.GetMaxHealth();
            if (_maxHealth != maxHealth)
            {
                _maxHealth = maxHealth;
                if (_healthContainer != null)
                    Destroy(_healthContainer);
                SetupHealthBar(FindObjectOfType<Canvas>());
            }
            else
            {
                RefreshHealthSegments(Player.Instantce.GetCurrentHealth());
            }

            Player.Instantce.OnPlayerDamaged += UpdateHealthBar;
            Player.Instantce.OnPlayerDeath += OnPlayerDeath;
            EnemyEntity.OnAnyEnemyDeath += OnEnemyDeath;
            _subscribed = true;
        }

        UpdateKillCounter();
    }

    private void Start()
    {
        SubscribeToPlayer();
    }

    private void UpdateHealthBar(object sender, System.EventArgs e)
    {
        if (Player.Instantce != null)
            RefreshHealthSegments(Player.Instantce.GetCurrentHealth());
    }

    private void OnPlayerDeath(object sender, System.EventArgs e)
    {
    }

    private void OnEnemyDeath(object sender, System.EventArgs e)
    {
        killCount++;
        UpdateKillCounter();
    }

    private void UpdateKillCounter()
    {
        if (killCounterText != null)
            killCounterText.text = $"Kills: {killCount}";
    }

    private void OnDestroy()
    {
        if (_subscribed)
        {
            if (Player.Instantce != null)
            {
                Player.Instantce.OnPlayerDamaged -= UpdateHealthBar;
                Player.Instantce.OnPlayerDeath -= OnPlayerDeath;
            }
            EnemyEntity.OnAnyEnemyDeath -= OnEnemyDeath;
        }
    }
}
