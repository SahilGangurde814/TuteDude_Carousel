using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class carouselPagination : MonoBehaviour
{
    public GameObject imagePrefab; // Prefab of the image to be added to the carousel.
    public Transform carouselPanelContainer; // The container holding all the panels.
    public GameObject panelPrefab; // Prefab for a panel (group of images).
    public int imagesPerPanel = 5; // Limit of images per panel.
    public Button nextButton, previousButton; // UI buttons to navigate between panels.
    public TextMeshProUGUI curentPanelCountText;

    // Full-screen display components
    public GameObject fullScreenImageContainer; // The container for the full-screen image.
    public Image fullScreenImage; // The actual image displayed full-screen.

    // Array of images to choose from
    public Sprite[] availableImages; // Array of different images.
    public Button[] imageButtons; // Buttons for adding different images.

    private List<GameObject> panels = new List<GameObject>(); // List to store panels.
    private int currentPanelIndex = 0;

    public Animator animator;

    void Start()
    {
        CreateNewPanel(); // Initialize by creating the first panel.
        UpdateNavigationButtons();

        // Assign image buttons to add specific images
        for (int i = 0; i < imageButtons.Length; i++)
        {
            int index = i; // Capture the index for the listener
            imageButtons[i].onClick.AddListener(() => AddImageToCarousel(index));
        }
    }

    public void AddImageToCarousel(int imageIndex)
    {
        // Get the last panel.
        GameObject currentPanel = panels[panels.Count - 1];

        // Check if the current panel has space for more images.
        if (currentPanel.transform.childCount >= imagesPerPanel)
        {
            // If the current panel is full, create a new panel.
            CreateNewPanel();
            currentPanel = panels[panels.Count - 1];
        }

        // Instantiate the new image and add it to the current panel.
        GameObject newImage = Instantiate(imagePrefab, currentPanel.transform);
        Button imageButton = newImage.GetComponent<Button>();

        // Set the image sprite based on the selected index
        Image imgComponent = newImage.GetComponent<Image>();
        imgComponent.sprite = availableImages[imageIndex];

        imageButton.onClick.AddListener(() => ShowFullScreenImage(newImage.GetComponent<Image>().sprite));

        UpdateNavigationButtons();
    }

    private void CreateNewPanel()
    {
        // Instantiate a new panel and add it to the carouselPanelContainer.
        GameObject newPanel = Instantiate(panelPrefab, carouselPanelContainer);
        newPanel.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleCenter; // Center alignment for images.
        newPanel.SetActive(false); // Disable the new panel by default.

        // Add the new panel to the list of panels.
        panels.Add(newPanel);

        // If this is the first panel, set it active.
        if (panels.Count == 1)
        {
            newPanel.SetActive(true);
        }
    }

    // Function to show the full-screen image.
    public void ShowFullScreenImage(Sprite imageSprite)
    {
        fullScreenImage.sprite = imageSprite; // Set the sprite of the full-screen image.
        fullScreenImageContainer.SetActive(true); // Show the full-screen container.

        //animator.Play("ImageZoomIn");
    }

    // Function to hide the full-screen image.
    public void HideFullScreenImage()
    {
        fullScreenImageContainer.SetActive(false); // Hide the full-screen container.
    }

    // Function to navigate to the next panel.
    public void NextPanel()
    {
        if (currentPanelIndex < panels.Count - 1)
        {
            panels[currentPanelIndex].SetActive(false);
            currentPanelIndex++;
            panels[currentPanelIndex].SetActive(true);
            UpdateNavigationButtons();
        }
    }

    // Function to navigate to the previous panel.
    public void PreviousPanel()
    {
        if (currentPanelIndex > 0)
        {
            panels[currentPanelIndex].SetActive(false);
            currentPanelIndex--;
            panels[currentPanelIndex].SetActive(true);
            UpdateNavigationButtons();
        }
    }

    // Enable/Disable navigation buttons based on the current panel index.
    private void UpdateNavigationButtons()
    {
        nextButton.interactable = (currentPanelIndex < panels.Count - 1);
        previousButton.interactable = (currentPanelIndex > 0);
        currentPanelCount();
    }

    void currentPanelCount()
    {
        curentPanelCountText.text = currentPanelIndex + 1 + " / " + panels.Count;
    }
}
